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
    [Route("api/CardTapHistories")]
    public class CardTapHistoriesController : Controller
    {
        private readonly ArtosDB _context;

        public CardTapHistoriesController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/CardTapHistories
        [HttpGet]
        public IEnumerable<CardTapHistory> GetCardTapHistorys()
        {
            return _context.CardTapHistorys;
        }

        // GET: api/CardTapHistories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardTapHistory([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cardTapHistory = await _context.CardTapHistorys.SingleOrDefaultAsync(m => m.Id == id);

            if (cardTapHistory == null)
            {
                return NotFound();
            }

            return Ok(cardTapHistory);
        }

        // PUT: api/CardTapHistories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardTapHistory([FromRoute] long id, [FromBody] CardTapHistory cardTapHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cardTapHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(cardTapHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardTapHistoryExists(id))
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

        // POST: api/CardTapHistories
        [HttpPost]
        public async Task<IActionResult> PostCardTapHistory([FromBody] CardTapHistory cardTapHistory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CardTapHistorys.Add(cardTapHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCardTapHistory", new { id = cardTapHistory.Id }, cardTapHistory);
        }

        // DELETE: api/CardTapHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardTapHistory([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cardTapHistory = await _context.CardTapHistorys.SingleOrDefaultAsync(m => m.Id == id);
            if (cardTapHistory == null)
            {
                return NotFound();
            }

            _context.CardTapHistorys.Remove(cardTapHistory);
            await _context.SaveChangesAsync();

            return Ok(cardTapHistory);
        }

        private bool CardTapHistoryExists(long id)
        {
            return _context.CardTapHistorys.Any(e => e.Id == id);
        }
    }
}