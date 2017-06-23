using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using ShoppingCartApi.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoppingCartApi.Repositories
{
    public class CartRepository : IDataAccess<Cart, int>
    {
        private ApiContext context;

        public CartRepository(ApiContext context){
            this.context = context;
        }

        public void Add(Cart b)
        {
            context.Carts.Add(b);
        }

        public void Delete(int id)
        {
            var cart = context.Carts.FirstOrDefault( x => x.RecordId == id );
            context.Carts.Remove(cart);
        }

        public Cart Get(int id)
        {
            return context.Carts.FirstOrDefault( x => x.RecordId == id );
        }

        public Cart Get(CartViewModel c)
        {
            return context.Carts.FirstOrDefault( x => x.AlbumId == c.AlbumId && x.CartId == c.CartId );
        }

        public IEnumerable<Cart> GetAll()
        {
            return context.Carts.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Cart b)
        {
            Cart cart = context.Carts.FirstOrDefault( x => x.RecordId == b.RecordId );
            if( cart != null ){
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