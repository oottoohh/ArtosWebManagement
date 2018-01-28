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
    [Route("api/GlobalConfigs")]
    public class GlobalConfigsController : Controller
    {
        private readonly ArtosDB _context;

        public GlobalConfigsController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/GlobalConfigs
        [HttpGet]
        public IEnumerable<GlobalConfig> GetGlobalConfigs()
        {
            return _context.GlobalConfigs;
        }

        // GET: api/GlobalConfigs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGlobalConfig([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var globalConfig = await _context.GlobalConfigs.SingleOrDefaultAsync(m => m.Id == id);

            if (globalConfig == null)
            {
                return NotFound();
            }

            return Ok(globalConfig);
        }

        // PUT: api/GlobalConfigs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGlobalConfig([FromRoute] long id, [FromBody] GlobalConfig globalConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != globalConfig.Id)
            {
                return BadRequest();
            }

            _context.Entry(globalConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GlobalConfigExists(id))
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

        // POST: api/GlobalConfigs
        [HttpPost]
        public async Task<IActionResult> PostGlobalConfig([FromBody] GlobalConfig globalConfig)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GlobalConfigs.Add(globalConfig);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGlobalConfig", new { id = globalConfig.Id }, globalConfig);
        }

        // DELETE: api/GlobalConfigs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGlobalConfig([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var globalConfig = await _context.GlobalConfigs.SingleOrDefaultAsync(m => m.Id == id);
            if (globalConfig == null)
            {
                return NotFound();
            }

            _context.GlobalConfigs.Remove(globalConfig);
            await _context.SaveChangesAsync();

            return Ok(globalConfig);
        }

        private bool GlobalConfigExists(long id)
        {
            return _context.GlobalConfigs.Any(e => e.Id == id);
        }
    }
}