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
    [Route("api/FAQs")]
    public class FAQsController : Controller
    {
        private readonly ArtosDB _context;

        public FAQsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/FAQs
        [HttpGet]
        public IEnumerable<FAQ> GetFAQs()
        {
            return _context.FAQs;
        }

        // GET: api/FAQs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFAQ([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fAQ = await _context.FAQs.SingleOrDefaultAsync(m => m.Id == id);

            if (fAQ == null)
            {
                return NotFound();
            }

            return Ok(fAQ);
        }

        // PUT: api/FAQs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFAQ([FromRoute] long id, [FromBody] FAQ fAQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != fAQ.Id)
            {
                return BadRequest();
            }

            _context.Entry(fAQ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FAQExists(id))
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

        // POST: api/FAQs
        [HttpPost]
        public async Task<IActionResult> PostFAQ([FromBody] FAQ fAQ)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.FAQs.Add(fAQ);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFAQ", new { id = fAQ.Id }, fAQ);
        }

        // DELETE: api/FAQs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFAQ([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fAQ = await _context.FAQs.SingleOrDefaultAsync(m => m.Id == id);
            if (fAQ == null)
            {
                return NotFound();
            }

            _context.FAQs.Remove(fAQ);
            await _context.SaveChangesAsync();

            return Ok(fAQ);
        }

        private bool FAQExists(long id)
        {
            return _context.FAQs.Any(e => e.Id == id);
        }
    }
}