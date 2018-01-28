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
    [Route("api/AppLogs")]
    public class AppLogsController : Controller
    {
        private readonly ArtosDB _context;

        public AppLogsController(ArtosDB context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/AppLogs
        [HttpGet]
        public IEnumerable<AppLog> GetAppLogs()
        {
            return _context.AppLogs;
        }

        // GET: api/AppLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appLog = await _context.AppLogs.SingleOrDefaultAsync(m => m.Id == id);

            if (appLog == null)
            {
                return NotFound();
            }

            return Ok(appLog);
        }

        // PUT: api/AppLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppLog([FromRoute] long id, [FromBody] AppLog appLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(appLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppLogExists(id))
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

        // POST: api/AppLogs
        [HttpPost]
        public async Task<IActionResult> PostAppLog([FromBody] AppLog appLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AppLogs.Add(appLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppLog", new { id = appLog.Id }, appLog);
        }

        // DELETE: api/AppLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appLog = await _context.AppLogs.SingleOrDefaultAsync(m => m.Id == id);
            if (appLog == null)
            {
                return NotFound();
            }

            _context.AppLogs.Remove(appLog);
            await _context.SaveChangesAsync();

            return Ok(appLog);
        }

        private bool AppLogExists(long id)
        {
            return _context.AppLogs.Any(e => e.Id == id);
        }
    }
}