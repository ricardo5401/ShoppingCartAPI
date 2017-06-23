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
    [Route("api/Artists")]
    public class ArtistsController : Controller
    {
        private ArtistRepository artistRepo;

        public ArtistsController(ApiContext context){
            artistRepo = new ArtistRepository(context);
        }

        // GET: api/Artists
        [HttpGet]
        public IEnumerable<Artist> GetArtists()
        {
            return artistRepo.GetAll();
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public IActionResult GetArtist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artist = artistRepo.Get(id);

            if (artist == null)
            {
                return NotFound();
            }

            return Ok(artist);
        }

        // PUT: api/Artists/5
        [HttpPut("{id}")]
        public IActionResult PutArtist([FromRoute] int id, [FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artist.ArtistId)
            {
                return BadRequest();
            }

            artistRepo.Update(artist);

            return Ok(artist);
        }

        // POST: api/Albums
        [HttpPost]
        public IActionResult PostArtist([FromBody] Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            artistRepo.Add(artist);

            return CreatedAtAction("GetAlbums", new { id = artist.ArtistId }, artist);
        }

        // DELETE: api/Albums/5
        [HttpDelete("{id}")]
        public IActionResult DeleteArtist([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artist = artistRepo.Get(id);
            if (artist == null)
            {
                return NotFound();
            }
            artistRepo.Delete(id);

            return Ok(artist);
        }

        private bool ArtistsExists(int id)
        {
            return artistRepo.Get(id) != null;
        }
    }
}
