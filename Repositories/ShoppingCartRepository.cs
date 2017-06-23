using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoppingCartApi.Repositories
{
    public class ShoppingCartRepository
    {
        private CartRepository cartRepo;
        private OrderDetailRepository orderDetailRepo;
        private OrderRepository orderRepo;

        public ShoppingCartRepository(ApiContext context)
        {
            cartRepo = new CartRepository(context);
            orderDetailRepo = new OrderDetailRepository(context);
            orderRepo = new OrderRepository(context);
        }

        public static ShoppingCartRepository GetCart(ApiContext context)
        {
            return new ShoppingCartRepository(context);
        }

        public Cart AddToCart(CartViewModel c)
        {
            return cartRepo.Add(c);
        }

        public int RemoveFromCart(CartDeleteViewModel c)
        {
            return cartRepo.Delete(c);
        }
        public void EmptyCart(string ShoppingCartId)
        {
            cartRepo.EmptyCart(ShoppingCartId);
        }
        public List<Cart> GetCartItems(string ShoppingCartId)
        {
            return cartRepo.GetAll(ShoppingCartId).ToList();
        }
        public int GetCount(string ShoppingCartId)
        {
            return cartRepo.GetCount(ShoppingCartId);
        }
        public decimal GetTotal(string ShoppingCartId)
        {
            return cartRepo.GetTotal(ShoppingCartId);
        }
        public int CreateOrder(OrderViewModel orderViewModel)
        {
            decimal orderTotal = 0;
            var order = orderRepo.Get(orderViewModel.OrderId);

            var cartItems = GetCartItems(orderViewModel.ShoppingCartId);

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Album.Price);

                orderDetailRepo.Add(orderDetail);

            }

            order.Total = orderTotal;
            orderRepo.Attach(order);
            orderDetailRepo.Save();
            orderRepo.Save();
            // Empty the shopping cart
            cartRepo.EmptyCart(orderViewModel.ShoppingCartId);
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
        public int CreateOrder(OrderPromoViewModel orderViewModel)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems(orderViewModel.ShoppingCartId);

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = orderViewModel.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };

                orderTotal += (item.Count * item.Album.Price);

                orderDetailRepo.Add(orderDetail);

            }
            // with PromoCode total price = 0
            // Save the orderDetails
            orderRepo.Save();
            orderDetailRepo.Save();
            // Empty the shopping cart
            cartRepo.EmptyCart(orderViewModel.ShoppingCartId);
            // Return the OrderId as the confirmation number
            return orderViewModel.OrderId;
        }
        
        public void MigrateCart(string userName)
        {
            var shoppingCart = cartRepo.GetAll(userName);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            cartRepo.Save();
        }
    }
}