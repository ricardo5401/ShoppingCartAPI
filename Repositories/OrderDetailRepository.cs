using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoppingCartApi.Repositories
{
    public class OrderDetailRepository : IDataAccess<OrderDetail, int>
    {
        private ApiContext context;

        public OrderDetailRepository(ApiContext context){
            this.context = context;
        }
        public void Add(OrderDetail b)
        {
            context.OrderDetails.Add(b);
        }

        public void Delete(int id)
        {
            OrderDetail genre = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);
            context.OrderDetails.Remove(genre);
        }

        public OrderDetail Get(int id)
        {
            return context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == id);
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return context.OrderDetails.ToList();
        }

        public IEnumerable<OrderDetail> GetAll(int id)
        {
            return context.OrderDetails.Where(x => x.OrderId == id).ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(OrderDetail b)
        {
            OrderDetail album = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == b.OrderDetailId);
            if(album != null){
                album.AlbumId= b.AlbumId;
                album.OrderId= b.OrderId;
                album.Quantity = b.Quantity;
                album.UnitPrice = b.UnitPrice;
                this.Save();
            }
        }      
    }
}