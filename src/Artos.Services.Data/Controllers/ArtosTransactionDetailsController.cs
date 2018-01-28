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
    [Route("api/ArtosTransactionDetails")]
    public class ArtosTransactionDetailsController : Controller
    {
        private readonly ArtosDB _context;

        public ArtosTransactionDetailsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/ArtosTransactionDetails
        [HttpGet]
        public IEnumerable<ArtosTransactionDetail> GetArtosTransactionDetails()
        {
            return _context.ArtosTransactionDetails;
        }

        // GET: api/ArtosTransactionDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArtosTransactionDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artosTransactionDetail = await _context.ArtosTransactionDetails.SingleOrDefaultAsync(m => m.Id == id);

            if (artosTransactionDetail == null)
            {
                return NotFound();
            }

            return Ok(artosTransactionDetail);
        }

        // PUT: api/ArtosTransactionDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtosTransactionDetail([FromRoute] long id, [FromBody] ArtosTransactionDetail artosTransactionDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != artosTransactionDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(artosTransactionDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtosTransactionDetailExists(id))
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

        // POST: api/ArtosTransactionDetails
        [HttpPost]
        public async Task<IActionResult> PostArtosTransactionDetail([FromBody] ArtosTransactionDetail artosTransactionDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ArtosTransactionDetails.Add(artosTransactionDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtosTransactionDetail", new { id = artosTransactionDetail.Id }, artosTransactionDetail);
        }

        // DELETE: api/ArtosTransactionDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtosTransactionDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var artosTransactionDetail = await _context.ArtosTransactionDetails.SingleOrDefaultAsync(m => m.Id == id);
            if (artosTransactionDetail == null)
            {
                return NotFound();
            }

            _context.ArtosTransactionDetails.Remove(artosTransactionDetail);
            await _context.SaveChangesAsync();

            return Ok(artosTransactionDetail);
        }

        private bool ArtosTransactionDetailExists(long id)
        {
            return _context.ArtosTransactionDetails.Any(e => e.Id == id);
        }
    }
}