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
    [Route("api/EMoneys")]
    public class EMoneysController : Controller
    {
        private readonly ArtosDB _context;

        public EMoneysController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/EMoneys
        [HttpGet]
        public IEnumerable<EMoney> GetEMoneys()
        {
            return _context.EMoneys;
        }

        // GET: api/EMoneys/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEMoney([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eMoney = await _context.EMoneys.SingleOrDefaultAsync(m => m.Id == id);

            if (eMoney == null)
            {
                return NotFound();
            }

            return Ok(eMoney);
        }

        // PUT: api/EMoneys/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEMoney([FromRoute] long id, [FromBody] EMoney eMoney)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eMoney.Id)
            {
                return BadRequest();
            }

            _context.Entry(eMoney).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EMoneyExists(id))
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

        // POST: api/EMoneys
        [HttpPost]
        public async Task<IActionResult> PostEMoney([FromBody] EMoney eMoney)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EMoneys.Add(eMoney);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEMoney", new { id = eMoney.Id }, eMoney);
        }

        // DELETE: api/EMoneys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEMoney([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eMoney = await _context.EMoneys.SingleOrDefaultAsync(m => m.Id == id);
            if (eMoney == null)
            {
                return NotFound();
            }

            _context.EMoneys.Remove(eMoney);
            await _context.SaveChangesAsync();

            return Ok(eMoney);
        }

        private bool EMoneyExists(long id)
        {
            return _context.EMoneys.Any(e => e.Id == id);
        }
    }
}