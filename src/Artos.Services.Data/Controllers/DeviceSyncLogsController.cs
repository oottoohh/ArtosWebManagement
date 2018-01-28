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
    [Route("api/DeviceSyncLogs")]
    public class DeviceSyncLogsController : Controller
    {
        private readonly ArtosDB _context;

        public DeviceSyncLogsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/DeviceSyncLogs
        [HttpGet]
        public IEnumerable<DeviceSyncLog> GetDeviceSyncLogs()
        {
            return _context.DeviceSyncLogs;
        }

        // GET: api/DeviceSyncLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceSyncLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceSyncLog = await _context.DeviceSyncLogs.SingleOrDefaultAsync(m => m.Id == id);

            if (deviceSyncLog == null)
            {
                return NotFound();
            }

            return Ok(deviceSyncLog);
        }

        // PUT: api/DeviceSyncLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeviceSyncLog([FromRoute] long id, [FromBody] DeviceSyncLog deviceSyncLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deviceSyncLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(deviceSyncLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceSyncLogExists(id))
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

        // POST: api/DeviceSyncLogs
        [HttpPost]
        public async Task<IActionResult> PostDeviceSyncLog([FromBody] DeviceSyncLog deviceSyncLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DeviceSyncLogs.Add(deviceSyncLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeviceSyncLog", new { id = deviceSyncLog.Id }, deviceSyncLog);
        }

        // DELETE: api/DeviceSyncLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeviceSyncLog([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deviceSyncLog = await _context.DeviceSyncLogs.SingleOrDefaultAsync(m => m.Id == id);
            if (deviceSyncLog == null)
            {
                return NotFound();
            }

            _context.DeviceSyncLogs.Remove(deviceSyncLog);
            await _context.SaveChangesAsync();

            return Ok(deviceSyncLog);
        }

        private bool DeviceSyncLogExists(long id)
        {
            return _context.DeviceSyncLogs.Any(e => e.Id == id);
        }
    }
}