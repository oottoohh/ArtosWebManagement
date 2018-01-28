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
    [Route("api/LanguageResources")]
    public class LanguageResourcesController : Controller
    {
        private readonly ArtosDB _context;

        public LanguageResourcesController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/LanguageResources
        [HttpGet]
        public IEnumerable<LanguageResource> GetLanguageResources()
        {
            return _context.LanguageResources;
        }

        // GET: api/LanguageResources/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLanguageResource([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var languageResource = await _context.LanguageResources.SingleOrDefaultAsync(m => m.Id == id);

            if (languageResource == null)
            {
                return NotFound();
            }

            return Ok(languageResource);
        }

        // PUT: api/LanguageResources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguageResource([FromRoute] long id, [FromBody] LanguageResource languageResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != languageResource.Id)
            {
                return BadRequest();
            }

            _context.Entry(languageResource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageResourceExists(id))
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

        // POST: api/LanguageResources
        [HttpPost]
        public async Task<IActionResult> PostLanguageResource([FromBody] LanguageResource languageResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.LanguageResources.Add(languageResource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLanguageResource", new { id = languageResource.Id }, languageResource);
        }

        // DELETE: api/LanguageResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguageResource([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var languageResource = await _context.LanguageResources.SingleOrDefaultAsync(m => m.Id == id);
            if (languageResource == null)
            {
                return NotFound();
            }

            _context.LanguageResources.Remove(languageResource);
            await _context.SaveChangesAsync();

            return Ok(languageResource);
        }

        private bool LanguageResourceExists(long id)
        {
            return _context.LanguageResources.Any(e => e.Id == id);
        }
    }
}