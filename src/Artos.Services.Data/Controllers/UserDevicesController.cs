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
    [Route("api/UserDevices")]
    public class UserDevicesController : Controller
    {
        private readonly ArtosDB _context;

        public UserDevicesController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/UserDevices
        [HttpGet]
        public IEnumerable<UserDevice> GetUserDevices()
        {
            return _context.UserDevices;
        }

        // GET: api/UserDevices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDevice([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDevice = await _context.UserDevices.SingleOrDefaultAsync(m => m.Id == id);

            if (userDevice == null)
            {
                return NotFound();
            }

            return Ok(userDevice);
        }

        // PUT: api/UserDevices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDevice([FromRoute] long id, [FromBody] UserDevice userDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDevice.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDevice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDeviceExists(id))
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

        // POST: api/UserDevices
        [HttpPost]
        public async Task<IActionResult> PostUserDevice([FromBody] UserDevice userDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserDevices.Add(userDevice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDevice", new { id = userDevice.Id }, userDevice);
        }

        // DELETE: api/UserDevices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDevice([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDevice = await _context.UserDevices.SingleOrDefaultAsync(m => m.Id == id);
            if (userDevice == null)
            {
                return NotFound();
            }

            _context.UserDevices.Remove(userDevice);
            await _context.SaveChangesAsync();

            return Ok(userDevice);
        }

        private bool UserDeviceExists(long id)
        {
            return _context.UserDevices.Any(e => e.Id == id);
        }
    }
}