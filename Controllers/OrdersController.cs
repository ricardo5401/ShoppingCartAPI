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
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private OrderRepository orederRepo;
        private OrderDetailRepository detailRepo;

        public OrdersController(ApiContext context){
            orederRepo = new OrderRepository(context);
            detailRepo = new OrderDetailRepository(context);
        }

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine("OrdersController - Index");
            return Ok(orederRepo.GetAll());
        }

        [HttpGet("{ShoppingCartId}")]
        public IActionResult Index(string ShoppingCartId)
        {
            Console.WriteLine("OrdersController - Index Id: " + ShoppingCartId);
            return Ok(orederRepo.GetAll(ShoppingCartId));
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            Console.WriteLine("OrdersController - Index OrderId: " + id);
            return Ok(detailRepo.GetAll(id));
        }
    }
}