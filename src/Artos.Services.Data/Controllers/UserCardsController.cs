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
    [Route("api/UserCards")]
    public class UserCardsController : Controller
    {
        private readonly ArtosDB _context;

        public UserCardsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/UserCards
        [HttpGet]
        public IEnumerable<UserCard> GetUserCards()
        {
            return _context.UserCards;
        }

        // GET: api/UserCards/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserCard([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userCard = await _context.UserCards.SingleOrDefaultAsync(m => m.Id == id);

            if (userCard == null)
            {
                return NotFound();
            }

            return Ok(userCard);
        }

        // PUT: api/UserCards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCard([FromRoute] long id, [FromBody] UserCard userCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userCard.Id)
            {
                return BadRequest();
            }

            _context.Entry(userCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCardExists(id))
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

        // POST: api/UserCards
        [HttpPost]
        public async Task<IActionResult> PostUserCard([FromBody] UserCard userCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserCards.Add(userCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserCard", new { id = userCard.Id }, userCard);
        }

        // DELETE: api/UserCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCard([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userCard = await _context.UserCards.SingleOrDefaultAsync(m => m.Id == id);
            if (userCard == null)
            {
                return NotFound();
            }

            _context.UserCards.Remove(userCard);
            await _context.SaveChangesAsync();

            return Ok(userCard);
        }

        private bool UserCardExists(long id)
        {
            return _context.UserCards.Any(e => e.Id == id);
        }
    }
}