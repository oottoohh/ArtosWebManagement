using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Artos.Entities;
using Artos.Services.Data.Helpers;

namespace Artos.Services.Data.Controllers
{
    [Produces("application/json")]
    [Route("api/Banners")]
    public class BannersController : Controller
    {
        private readonly ArtosDB _context;

        public BannersController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/Banners
        [HttpGet]
        public IEnumerable<Banner> GetBanners()
        {
            return _context.Banners;
        }

        // GET: api/Banners/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanner([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var banner = await _context.Banners.SingleOrDefaultAsync(m => m.Id == id);

            if (banner == null)
            {
                return NotFound();
            }

            return Ok(banner);
        }

        // PUT: api/Banners/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBanner([FromRoute] long id, [FromBody] Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != banner.Id)
            {
                return BadRequest();
            }

            _context.Entry(banner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BannerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Banners
        [HttpPost]
        public async Task<IActionResult> PostBanner([FromBody] Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBanner", new { id = banner.Id }, banner);
        }

        // DELETE: api/Banners/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var banner = await _context.Banners.SingleOrDefaultAsync(m => m.Id == id);
            if (banner == null)
            {
                return NotFound();
            }

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return Ok(banner);
        }

        private bool BannerExists(long id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}