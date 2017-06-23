using ShoppingCartApi.Models;
using ShoppingCartApi.ViewModels;
using ShoppingCartApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ShoppingCartApi.Controllers
{
    [Authorize]
    [Route("api/ShoppingCarts")]
    public class ShoppingCartsController : Controller
    {
        private ApiContext storeDB = new ApiContext();
        
        public IActionResult Index([FromBody] string ShoppingCartId)
        {
            var cart = ShoppingCartRepository.GetCart(storeDB);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(ShoppingCartId),
                CartTotal = cart.GetTotal(ShoppingCartId)
            };
            // Return the view
            return Ok(viewModel);
        }
        
        public IActionResult AddToCart([FromBody] CartViewModel c)
        {
            // Retrieve the album from the database
            var addedAlbum = storeDB.Albums
                .Single(album => album.AlbumId == c.AlbumId);

            // Add it to the shopping cart
            var cart = ShoppingCartRepository.GetCart(storeDB);

            cart.AddToCart(c);

            // Go back to the main store page for more shopping
            return Ok(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] CartDeleteViewModel c)
        {
            // Remove the item from the cart
            var cart = ShoppingCartRepository.GetCart(storeDB);

            // Get the name of the album to display confirmation
            string albumName = storeDB.Carts
                .Single(item => item.RecordId == c.RecordId).Album.Title;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(c);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = albumName +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(c.ShoppingCartId),
                CartCount = cart.GetCount(c.ShoppingCartId),
                ItemCount = itemCount,
                DeleteId = c.RecordId
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        public IActionResult CartSummary([FromBody] string ShoppingCartId)
        {
            var cart = ShoppingCartRepository.GetCart(storeDB);
            return Ok(cart.GetCount(ShoppingCartId));
        }
        
    }
}