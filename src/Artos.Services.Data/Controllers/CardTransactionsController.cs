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
    [Route("api/CardTransactions")]
    public class CardTransactionsController : Controller
    {
        private readonly ArtosDB _context;

        public CardTransactionsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/CardTransactions
        [HttpGet]
        public IEnumerable<CardTransaction> GetCardTransactions()
        {
            return _context.CardTransactions;
        }

        // GET: api/CardTransactions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardTransaction([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cardTransaction = await _context.CardTransactions.SingleOrDefaultAsync(m => m.Id == id);

            if (cardTransaction == null)
            {
                return NotFound();
            }

            return Ok(cardTransaction);
        }

        // PUT: api/CardTransactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardTransaction([FromRoute] long id, [FromBody] CardTransaction cardTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cardTransaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(cardTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardTransactionExists(id))
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

        // POST: api/CardTransactions
        [HttpPost]
        public async Task<IActionResult> PostCardTransaction([FromBody] CardTransaction cardTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CardTransactions.Add(cardTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCardTransaction", new { id = cardTransaction.Id }, cardTransaction);
        }

        // DELETE: api/CardTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardTransaction([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cardTransaction = await _context.CardTransactions.SingleOrDefaultAsync(m => m.Id == id);
            if (cardTransaction == null)
            {
                return NotFound();
            }

            _context.CardTransactions.Remove(cardTransaction);
            await _context.SaveChangesAsync();

            return Ok(cardTransaction);
        }

        private bool CardTransactionExists(long id)
        {
            return _context.CardTransactions.Any(e => e.Id == id);
        }
    }
}