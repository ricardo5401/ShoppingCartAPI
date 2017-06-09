using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoppingCartApi.Repositories
{
    public class ArtistRepository : IDataAccess<Artist, int>
    {
        private ApiContext context;

        public ArtistRepository(ApiContext context){
            this.context = context;
        }
        public void Add(Artist b)
        {
            context.Artists.Add(b);
        }

        public void Delete(int id)
        {
            Artist artist = context.Artists.FirstOrDefault(x => x.ArtistId == id);
            context.Artists.Remove(artist);
        }

        public Artist Get(int id)
        {
            return context.Artists.FirstOrDefault(x => x.ArtistId == id);
        }

        public IEnumerable<Artist> GetAll()
        {
            return context.Artists.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Artist b)
        {
            Artist artist = context.Artists.FirstOrDefault(x => x.ArtistId == b.ArtistId);
            if(artist != null){
                artist.Name = b.Name;
                this.Save();
            }
        }      
    }
}