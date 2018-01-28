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
    [Route("api/Routes")]
    public class RoutesController : Controller
    {
        private readonly ArtosDB _context;

        public RoutesController(ArtosDB context)
        {
            _context = context;
        }

        // GET: api/Routes
        [HttpGet]
        public IEnumerable<Route> GetRoute()
        {
            return _context.Route;
        }

        // GET: api/Routes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoute([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var route = await _context.Route.SingleOrDefaultAsync(m => m.Id == id);

            if (route == null)
            {
                return NotFound();
            }

            return Ok(route);
        }

        // PUT: api/Routes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoute([FromRoute] long id, [FromBody] Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != route.Id)
            {
                return BadRequest();
            }

            _context.Entry(route).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RouteExists(id))
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

        // POST: api/Routes
        [HttpPost]
        public async Task<IActionResult> PostRoute([FromBody] Route route)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Route.Add(route);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoute", new { id = route.Id }, route);
        }

        // DELETE: api/Routes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoute([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var route = await _context.Route.SingleOrDefaultAsync(m => m.Id == id);
            if (route == null)
            {
                return NotFound();
            }

            _context.Route.Remove(route);
            await _context.SaveChangesAsync();

            return Ok(route);
        }

        private bool RouteExists(long id)
        {
            return _context.Route.Any(e => e.Id == id);
        }
    }
}