using System;
using System.Linq;
using ShoppingCartApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCartApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApiContext context)
        {
            if(context.Genres.Any())
            {
                return; //DB has been seeded
            }
            var artists = new Artist[]
            {
                new Artist { ArtistId = 1, Name = "Donna Summer" },
                new Artist { ArtistId = 2, Name = "Spyro Gira" },
                new Artist { ArtistId = 3, Name = "Emy WineHouse" }
            };
            foreach(Artist a in artists){
                context.Artists.Add(a);
            }
            context.SaveChanges();
            var genres = new Genre[]
            {
                new Genre { GenreId = 1, Name = "Disco", Description = "Music disco" },
                new Genre { GenreId = 2, Name = "Pop", Description = "Dancing Pop" }
            };
            foreach(Genre g in genres){
                context.Genres.Add(g);
            }
            context.SaveChanges();
            var albums = new Album[]
            {

                new Album { AlbumId = 1, ArtistId = 1, GenreId = 1, Title = "Jon Galloway", Price = 5, AlbumArtUrl = "https://images.pexels.com/photos/57816/pexels-photo-57816.jpeg?h=350&auto=compress&cs=tinysrgb" },
                new Album { AlbumId = 2, ArtistId = 2, GenreId = 1, Title = "The Essencial Miles", Price = 8, AlbumArtUrl = "https://images.pexels.com/photos/172105/pexels-photo-172105.jpeg?w=940&h=650&auto=compress&cs=tinysrgb" },
                new Album { AlbumId = 3, ArtistId = 3, GenreId = 2, Title = "Frank", Price = 10, AlbumArtUrl = "https://images.pexels.com/photos/77414/nostalgy-collector-theme-good-morning-retro-old-technology-77414.jpeg?w=940&h=650&auto=compress&cs=tinysrgb" }
            };
            foreach(Album a in albums){
                context.Albums.Add(a);
            }
            context.SaveChanges();
        }
    }
}