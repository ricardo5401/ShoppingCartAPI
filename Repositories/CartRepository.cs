using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartApi.Repositories
{
    public class CartRepository : IDataAccess<Cart, int>
    {
        private ApiContext context;

        public CartRepository(ApiContext context)
        {
            this.context = context;
        }

        public void Add(Cart b)
        {
            context.Carts.Add(b);
        }

        public Cart Add(CartViewModel c)
        {
            var cart = this.Get(c);
            if (cart == null)
            {
                cart = new Cart
                {
                    AlbumId = c.AlbumId,
                    CartId = c.CartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                context.Carts.Add(cart);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cart.Count++;
            }
            this.Save();
            // return complete model with id
            return this.Get(c);
        }

        public void Delete(int id)
        {
            var cart = context.Carts.FirstOrDefault(x => x.RecordId == id);
            context.Carts.Remove(cart);
        }

        public int Delete(CartDeleteViewModel c)
        {
            var cartItem = this.Get(c);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    context.Carts.Remove(cartItem);
                }
                // Save changes
                this.Save();
            }
            return itemCount;
        }

        public void EmptyCart(string ShoppingCartId)
        {
            var cartItems = context.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                context.Carts.Remove(cartItem);
            }
            // Save changes
            this.Save();
        }

        public Cart Get(int id)
        {
            return context.Carts.FirstOrDefault(x => x.RecordId == id);
        }

        public Cart Get(CartViewModel c)
        {
            return context.Carts.FirstOrDefault(x => x.AlbumId == c.AlbumId && x.CartId == c.CartId);
        }

        public Cart Get(CartDeleteViewModel c)
        {
            return context.Carts.FirstOrDefault(x => x.RecordId == c.RecordId && x.CartId == c.ShoppingCartId);
        }

        public IEnumerable<Cart> GetAll()
        {
            return context.Carts.ToList();
        }

        public IEnumerable<Cart> GetAll(string ShoppingCartId)
        {
            return context.Carts.Where(
                c => c.CartId == ShoppingCartId).ToList();
        }
        public int GetCount(string ShoppingCartId)
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in context.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int)cartItems.Count).Sum();
            // Return 0 if all entries are null
            Console.WriteLine("CartRepository, GetCount: " + count);
            return count ?? 0;
        }
        public decimal GetTotal(string ShoppingCartId)
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in context.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.Album.Price).Sum();

            return total ?? decimal.Zero;
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Cart b)
        {
            Cart cart = context.Carts.FirstOrDefault(x => x.RecordId == b.RecordId);
            if (cart != null)
            {
                cart.AlbumId = b.AlbumId;
                cart.CartId = b.CartId;
                cart.Count = b.Count;
                cart.DateCreated = b.DateCreated;
                cart.Album = b.Album;
                this.Save();
            }
        }
    }
}