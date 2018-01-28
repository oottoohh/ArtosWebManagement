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
    [Route("api/OTPs")]
    public class OTPsController : Controller
    {
        private readonly ArtosDB _context;

        public OTPsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/OTPs
        [HttpGet]
        public IEnumerable<OTP> GetOTPs()
        {
            return _context.OTPs;
        }

        // GET: api/OTPs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOTP([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oTP = await _context.OTPs.SingleOrDefaultAsync(m => m.Id == id);

            if (oTP == null)
            {
                return NotFound();
            }

            return Ok(oTP);
        }

        // PUT: api/OTPs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOTP([FromRoute] long id, [FromBody] OTP oTP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oTP.Id)
            {
                return BadRequest();
            }

            _context.Entry(oTP).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OTPExists(id))
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

        // POST: api/OTPs
        [HttpPost]
        public async Task<IActionResult> PostOTP([FromBody] OTP oTP)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.OTPs.Add(oTP);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOTP", new { id = oTP.Id }, oTP);
        }

        // DELETE: api/OTPs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOTP([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var oTP = await _context.OTPs.SingleOrDefaultAsync(m => m.Id == id);
            if (oTP == null)
            {
                return NotFound();
            }

            _context.OTPs.Remove(oTP);
            await _context.SaveChangesAsync();

            return Ok(oTP);
        }

        private bool OTPExists(long id)
        {
            return _context.OTPs.Any(e => e.Id == id);
        }
    }
}