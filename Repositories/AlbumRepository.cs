using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoppingCartApi.Repositories
{
    public class AlbumRepository : IDataAccess<Album, int>
    {
        private ApiContext context;

        public AlbumRepository(ApiContext context){
            this.context = context;
        }
        public void Add(Album b)
        {
            context.Albums.Add(b);
        }

        public void Delete(int id)
        {
            Album album = context.Albums.FirstOrDefault(x => x.AlbumId == id);
            context.Albums.Remove(album);
        }

        public Album Get(int id)
        {
            return context.Albums.FirstOrDefault(x => x.AlbumId == id);
        }

        public IEnumerable<Album> GetAll()
        {
            return context.Albums.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Album b)
        {
            Album album = context.Albums.FirstOrDefault(x => x.AlbumId == b.AlbumId);
            if(album != null){
                album.AlbumArtUrl = b.AlbumArtUrl;
                album.ArtistId = b.ArtistId;
                album.GenreId = b.GenreId;
                album.Price = b.Price;
                album.Title = b.Title;
                this.Save();
            }
        }      
    }
}