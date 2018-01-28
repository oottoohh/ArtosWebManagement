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
    [Route("api/StaticContents")]
    public class StaticContentsController : Controller
    {
        private readonly ArtosDB _context;

        public StaticContentsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/StaticContents
        [HttpGet]
        public IEnumerable<StaticContent> GetStaticContents()
        {
            return _context.StaticContents;
        }

        // GET: api/StaticContents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaticContent([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staticContent = await _context.StaticContents.SingleOrDefaultAsync(m => m.Id == id);

            if (staticContent == null)
            {
                return NotFound();
            }

            return Ok(staticContent);
        }

        // PUT: api/StaticContents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaticContent([FromRoute] string id, [FromBody] StaticContent staticContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != staticContent.Id)
            {
                return BadRequest();
            }

            _context.Entry(staticContent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaticContentExists(id))
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

        // POST: api/StaticContents
        [HttpPost]
        public async Task<IActionResult> PostStaticContent([FromBody] StaticContent staticContent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StaticContents.Add(staticContent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaticContent", new { id = staticContent.Id }, staticContent);
        }

        // DELETE: api/StaticContents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaticContent([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staticContent = await _context.StaticContents.SingleOrDefaultAsync(m => m.Id == id);
            if (staticContent == null)
            {
                return NotFound();
            }

            _context.StaticContents.Remove(staticContent);
            await _context.SaveChangesAsync();

            return Ok(staticContent);
        }

        private bool StaticContentExists(string id)
        {
            return _context.StaticContents.Any(e => e.Id == id);
        }
    }
}