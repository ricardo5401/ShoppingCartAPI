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
        private AlbumRepository albumRepo;

        public ShoppingCartRepository(ApiContext context)
        {
            cartRepo = new CartRepository(context);
            orderDetailRepo = new OrderDetailRepository(context);
            orderRepo = new OrderRepository(context);
            albumRepo = new AlbumRepository(context);
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
        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems(order.Username);

            foreach (var item in cartItems)
            {
                var album = albumRepo.Get(item.AlbumId);
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = album.Price,
                    Quantity = item.Count
                };
                orderTotal += (item.Count * album.Price);
                orderDetailRepo.Add(orderDetail);
            }
            order.Total = orderTotal;
            orderRepo.Attach(order);
            orderDetailRepo.Save();
            orderRepo.Save();
            // Empty the shopping cart
            cartRepo.EmptyCart(order.Username);
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
        
        public void MigrateCart(MigrateCardViewModel model)
        {
            var shoppingCart = cartRepo.GetAll(model.OldCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = model.CartId;
            }
            cartRepo.Save();
        }
    }
}