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
    [Route("api/ContactCenters")]
    public class ContactCentersController : Controller
    {
        private readonly ArtosDB _context;

        public ContactCentersController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/ContactCenters
        [HttpGet]
        public IEnumerable<ContactCenter> GetContactCenters()
        {
            return _context.ContactCenters;
        }

        // GET: api/ContactCenters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactCenter([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactCenter = await _context.ContactCenters.SingleOrDefaultAsync(m => m.Id == id);

            if (contactCenter == null)
            {
                return NotFound();
            }

            return Ok(contactCenter);
        }

        // PUT: api/ContactCenters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactCenter([FromRoute] long id, [FromBody] ContactCenter contactCenter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactCenter.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactCenter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactCenterExists(id))
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

        // POST: api/ContactCenters
        [HttpPost]
        public async Task<IActionResult> PostContactCenter([FromBody] ContactCenter contactCenter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ContactCenters.Add(contactCenter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactCenter", new { id = contactCenter.Id }, contactCenter);
        }

        // DELETE: api/ContactCenters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactCenter([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactCenter = await _context.ContactCenters.SingleOrDefaultAsync(m => m.Id == id);
            if (contactCenter == null)
            {
                return NotFound();
            }

            _context.ContactCenters.Remove(contactCenter);
            await _context.SaveChangesAsync();

            return Ok(contactCenter);
        }

        private bool ContactCenterExists(long id)
        {
            return _context.ContactCenters.Any(e => e.Id == id);
        }
    }
}