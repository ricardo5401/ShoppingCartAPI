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
    [Route("api/Checkout")]
    public class CheckoutController : Controller
    {
        private ApiContext storeDB;
        const string PromoCode = "FREE";

        public CheckoutController(ApiContext context){
            this.storeDB = context;
        }

        [HttpGet("AddressAndPayment")]
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

        [HttpPost("AddressAndPayment")]
        public IActionResult AddressAndPayment([FromBody] OrderFormViewModel model)
        {
            var order = model.order;
            order.Username = model.shoppingCartId;
            order.OrderDate = DateTime.Now;
            Console.WriteLine("AddresPayment - ShoppingCartId: " + model.shoppingCartId);
            //Save Order
            storeDB.Orders.Add(order);
            storeDB.SaveChanges();
            //Process the order
            var cart = ShoppingCartRepository.GetCart(storeDB);
            cart.CreateOrder(order);

            Console.WriteLine("AddresPayment - OrderId: " + order.OrderId);
            return Ok(order);
        }

        // GET: /Checkout/Complete

        [HttpGet("Complete")]
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

        [HttpPost("MigrateCart")]
        public IActionResult MigrateCart([FromBody] MigrateCardViewModel model)
        {
            var cart = ShoppingCartRepository.GetCart(storeDB);
            cart.MigrateCart(model);

            return Ok();
        }
    }
}