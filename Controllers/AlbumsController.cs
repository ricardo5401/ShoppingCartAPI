using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.Repositories;

namespace ShoppingCartApi.Controllers
{
    [Route("api/Albums")]
    public class AlbumsController : Controller
    {
        private AlbumRepository albumRepo;

        public AlbumsController(ApiContext context){
            albumRepo = new AlbumRepository(context);
        }
        
        // GET: api/Albums
        [HttpGet]
        public IEnumerable<Album> GetAlbums()
        {
            return albumRepo.GetAll();
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        public IActionResult GetAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var album = albumRepo.Get(id);

            if (album == null)
            {
                return NotFound();
            }

            return Ok(album);
        }

        // PUT: api/Albums/5
        [HttpPut("{id}")]
        public IActionResult PutAlbums([FromRoute] int id, [FromBody] Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != album.AlbumId)
            {
                return BadRequest();
            }

            albumRepo.Update(album);

            return NoContent();
        }

        // POST: api/Albums
        [HttpPost]
        public IActionResult PostAlbums([FromBody] Album album)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            albumRepo.Add(album);

            return CreatedAtAction("GetAlbums", new { id = album.AlbumId }, album);
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        public IActionResult DeleteAlbums([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var albums = albumRepo.Get(id);
            if (albums == null)
            {
                return NotFound();
            }
            albumRepo.Delete(id);

            return Ok(albums);
        }

        private bool AlbumsExists(int id)
        {
            return albumRepo.Get(id) != null;
        }
    }
}
