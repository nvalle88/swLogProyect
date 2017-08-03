using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swLogProyect.datos;
using bd.swLogProyect.entidades;

namespace bd.swLogProyect.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/LogLevels")]
    public class LogLevelsController : Controller
    {
        private readonly SwLogDbContext _context;

        public LogLevelsController(SwLogDbContext context)
        {
            _context = context;
        }

        // GET: api/LogLevels
        [HttpGet]
        public IEnumerable<LogLevel> GetLogLevels()
        {
            return _context.LogLevels;
        }

        // GET: api/LogLevels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogLevel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logLevel = await _context.LogLevels.SingleOrDefaultAsync(m => m.LogLevelId == id);

            if (logLevel == null)
            {
                return NotFound();
            }

            return Ok(logLevel);
        }

        // PUT: api/LogLevels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogLevel([FromRoute] int id, [FromBody] LogLevel logLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logLevel.LogLevelId)
            {
                return BadRequest();
            }

            _context.Entry(logLevel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogLevelExists(id))
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

        // POST: api/LogLevels
        [HttpPost]
        public async Task<IActionResult> PostLogLevel([FromBody] LogLevel logLevel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.LogLevels.Add(logLevel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogLevel", new { id = logLevel.LogLevelId }, logLevel);
        }

        // DELETE: api/LogLevels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogLevel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logLevel = await _context.LogLevels.SingleOrDefaultAsync(m => m.LogLevelId == id);
            if (logLevel == null)
            {
                return NotFound();
            }

            _context.LogLevels.Remove(logLevel);
            await _context.SaveChangesAsync();

            return Ok(logLevel);
        }

        private bool LogLevelExists(int id)
        {
            return _context.LogLevels.Any(e => e.LogLevelId == id);
        }
    }
}