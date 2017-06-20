using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoppingCartApi.Repositories
{
    public class GenreRepository : IDataAccess<Genre, int>
    {
        private ApiContext context;

        public GenreRepository(ApiContext context){
            this.context = context;
        }
        public void Add(Genre b)
        {
            context.Genres.Add(b);
        }

        public void Delete(int id)
        {
            Genre genre = context.Genres.FirstOrDefault(x => x.GenreId == id);
            context.Genres.Remove(genre);
        }

        public Genre Get(int id)
        {
            return context.Genres.FirstOrDefault(x => x.GenreId == id);
        }

        public IEnumerable<Genre> GetAll()
        {
            return context.Genres.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Genre b)
        {
            Genre album = context.Genres.FirstOrDefault(x => x.GenreId == b.GenreId);
            if(album != null){
                album.Description= b.Description;
                album.Name = b.Name;
                this.Save();
            }
        }      
    }
}