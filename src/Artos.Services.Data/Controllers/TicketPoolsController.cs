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
    [Route("api/TicketPools")]
    public class TicketPoolsController : Controller
    {
        private readonly ArtosDB _context;

        public TicketPoolsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/TicketPools
        [HttpGet]
        public IEnumerable<TicketPool> GetTicketPools()
        {
            return _context.TicketPools;
        }

        // GET: api/TicketPools/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketPool([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketPool = await _context.TicketPools.SingleOrDefaultAsync(m => m.Id == id);

            if (ticketPool == null)
            {
                return NotFound();
            }

            return Ok(ticketPool);
        }

        // PUT: api/TicketPools/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketPool([FromRoute] long id, [FromBody] TicketPool ticketPool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticketPool.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticketPool).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketPoolExists(id))
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

        // POST: api/TicketPools
        [HttpPost]
        public async Task<IActionResult> PostTicketPool([FromBody] TicketPool ticketPool)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TicketPools.Add(ticketPool);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicketPool", new { id = ticketPool.Id }, ticketPool);
        }

        // DELETE: api/TicketPools/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketPool([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketPool = await _context.TicketPools.SingleOrDefaultAsync(m => m.Id == id);
            if (ticketPool == null)
            {
                return NotFound();
            }

            _context.TicketPools.Remove(ticketPool);
            await _context.SaveChangesAsync();

            return Ok(ticketPool);
        }

        private bool TicketPoolExists(long id)
        {
            return _context.TicketPools.Any(e => e.Id == id);
        }
    }
}