using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoppingCartApi.Repositories
{
    public class OrderRepository : IDataAccess<Order, int>
    {
        private ApiContext context;

        public OrderRepository(ApiContext context){
            this.context = context;
        }
        public void Add(Order b)
        {
            context.Orders.Add(b);
        }

        public void Delete(int id)
        {
            Order genre = context.Orders.FirstOrDefault(x => x.OrderId == id);
            context.Orders.Remove(genre);
        }

        public Order Get(int id)
        {
            return context.Orders.FirstOrDefault(x => x.OrderId == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return context.Orders.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Order b)
        {
            Order album = context.Orders.FirstOrDefault(x => x.OrderId == b.OrderId);
            if(album != null){
                album.Address = b.Address;
                album.City =b.City;
                album.Country = b.Country;
                album.Email = b.Email;
                album.FirstName = b.FirstName;
                album.LastName = b.LastName;
                album.OrderDate = b.OrderDate;
                album.Phone = b.Phone;
                album.PostalCode = b.PostalCode;
                album.State = b.State; 
                this.Save();
            }
        }      
    }
}