using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.ViewModels;
using ShoppingCartApi.Data;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Controllers
{
    [Authorize]
    [Route("api/Checkouts")]
    public class CheckoutController : Controller
    {
        private ApiContext storeDB = new ApiContext();
        const string PromoCode = "FREE";

        public IActionResult AddressAndPayment([FromBody] UserViewModel user)
        {
            Order order = new Order();
            if(user != null)
            {
                order.Email = user.Email;
                order.FirstName = user.FirstName;
                order.LastName = user.LastName;
                order.Phone = user.PhoneNumber;
                order.City = user.City;
                order.Country = user.Country;
                order.PostalCode = user.PostalCode;
                order.State = user.City;
            }
            return Ok(order);
        }

        [HttpPost]
        public IActionResult AddressAndPayment([FromBody] Order order, [FromQuery] string ShoppingCartId, [FromQuery] string promoCode)
        {
            try
            {
                order.Username = ShoppingCartId;
                order.OrderDate = DateTime.Now;

                //Save Order
                storeDB.Orders.Add(order);
                storeDB.SaveChanges();
                //Process the order
                var cart = ShoppingCartRepository.GetCart(storeDB);
                var orderVM = new OrderViewModel { ShoppingCartId = ShoppingCartId, OrderId = order.OrderId };

                cart.CreateOrder(orderVM);
            }
            catch
            {
                //Invalid - redisplay with errors
                return BadRequest();
            }
            return Ok(order);
        }

        // GET: /Checkout/Complete
        public IActionResult Complete(OrderViewModel order)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == order.OrderId &&
                o.Username == order.ShoppingCartId);

            if (isValid)
            {
                return Ok(order.OrderId);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}