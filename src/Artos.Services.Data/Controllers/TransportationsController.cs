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
    [Route("api/Transportations")]
    public class TransportationsController : Controller
    {
        private readonly ArtosDB _context;

        public TransportationsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/Transportations
        [HttpGet]
        public IEnumerable<Transportation> GetTransportations()
        {
            return _context.Transportations;
        }

        // GET: api/Transportations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransportation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transportation = await _context.Transportations.SingleOrDefaultAsync(m => m.Id == id);

            if (transportation == null)
            {
                return NotFound();
            }

            return Ok(transportation);
        }

        // PUT: api/Transportations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransportation([FromRoute] long id, [FromBody] Transportation transportation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transportation.Id)
            {
                return BadRequest();
            }

            _context.Entry(transportation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportationExists(id))
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

        // POST: api/Transportations
        [HttpPost]
        public async Task<IActionResult> PostTransportation([FromBody] Transportation transportation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Transportations.Add(transportation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransportation", new { id = transportation.Id }, transportation);
        }

        // DELETE: api/Transportations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransportation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transportation = await _context.Transportations.SingleOrDefaultAsync(m => m.Id == id);
            if (transportation == null)
            {
                return NotFound();
            }

            _context.Transportations.Remove(transportation);
            await _context.SaveChangesAsync();

            return Ok(transportation);
        }

        private bool TransportationExists(long id)
        {
            return _context.Transportations.Any(e => e.Id == id);
        }
    }
}