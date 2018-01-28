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
    [Route("api/ArtosTransactions")]
    public class ArtosTransactionsController : Controller
    {
        private readonly ArtosDB _context;

        public ArtosTransactionsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/ArtosTransactions
        [HttpGet]
        public IEnumerable<ArtosTransaction> GetArtosTransactions()
        {
            return _context.ArtosTransactions;
        }

        // GET: api/ArtosTransactions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtosTransaction([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artosTransaction = await _context.ArtosTransactions.SingleOrDefaultAsync(m => m.Id == id);

            if (artosTransaction == null)
            {
                return NotFound();
            }

            return Ok(artosTransaction);
        }

        // PUT: api/ArtosTransactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtosTransaction([FromRoute] long id, [FromBody] ArtosTransaction artosTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artosTransaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(artosTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtosTransactionExists(id))
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

        // POST: api/ArtosTransactions
        [HttpPost]
        public async Task<IActionResult> PostArtosTransaction([FromBody] ArtosTransaction artosTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ArtosTransactions.Add(artosTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtosTransaction", new { id = artosTransaction.Id }, artosTransaction);
        }

        // DELETE: api/ArtosTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtosTransaction([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artosTransaction = await _context.ArtosTransactions.SingleOrDefaultAsync(m => m.Id == id);
            if (artosTransaction == null)
            {
                return NotFound();
            }

            _context.ArtosTransactions.Remove(artosTransaction);
            await _context.SaveChangesAsync();

            return Ok(artosTransaction);
        }

        private bool ArtosTransactionExists(long id)
        {
            return _context.ArtosTransactions.Any(e => e.Id == id);
        }
    }
}