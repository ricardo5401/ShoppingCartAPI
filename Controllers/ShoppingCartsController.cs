using ShoppingCartApi.Models;
using ShoppingCartApi.ViewModels;
using ShoppingCartApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace ShoppingCartApi.Controllers
{
    [Authorize]
    [Route("api/ShoppingCarts")]
    public class ShoppingCartsController : Controller
    {
        private ApiContext storeDB = new ApiContext();

        public ShoppingCartsController(ApiContext context){
            this.storeDB = context;
        }
        
        [HttpGet("{ShoppingCartId}")]
        public IActionResult Index(string ShoppingCartId)
        {
            Console.WriteLine("ShoppingCart Index, Id: " + ShoppingCartId);
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
        
        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] CartViewModel c)
        {

            Console.WriteLine("ShoppingCart AddToCard, Id: " + c.CartId);
            var cart = ShoppingCartRepository.GetCart(storeDB);
            var result = cart.AddToCart(c);
            Console.WriteLine(result);
            // Go back to the main store page for more shopping
            return Ok(result);
        }

        [HttpPost("RemoveFromCart")]
        public IActionResult RemoveFromCart([FromBody] CartDeleteViewModel c)
        {
            Console.WriteLine("ShoppingCart RemoveFromCart, Id: " + c.ShoppingCartId);
            // Remove the item from the cart
            var cart = ShoppingCartRepository.GetCart(storeDB);

            // Get the name of the album to display confirmation
            var record = storeDB.Carts
                .Single(item => item.RecordId == c.RecordId);
            Console.WriteLine("AlbumId: " + record.AlbumId);
            var albumName = new AlbumRepository(storeDB).Get(record.AlbumId).Title;
            Console.WriteLine("Album title: " + albumName);
            // Remove from cart
            int itemCount = cart.RemoveFromCart(c);
            Console.WriteLine("ShoppingCart Controller, itemCount: " + itemCount);
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
        [HttpGet("CartSummary/{CartId}")]
        public IActionResult CartSummary(string CartId)
        {
            var cart = ShoppingCartRepository.GetCart(storeDB);
            return Ok(cart.GetCount(CartId));
        }
        
    }
}