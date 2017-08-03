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
    [Route("api/LogCategories")]
    public class LogCategoriesController : Controller
    {
        private readonly SwLogDbContext _context;

        public LogCategoriesController(SwLogDbContext context)
        {
            _context = context;
        }

        // GET: api/LogCategories
        [HttpGet]
        public IEnumerable<LogCategory> GetLogCategories()
        {
            return _context.LogCategories;
        }

        // GET: api/LogCategories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logCategory = await _context.LogCategories.SingleOrDefaultAsync(m => m.LogCategoryId == id);

            if (logCategory == null)
            {
                return NotFound();
            }

            return Ok(logCategory);
        }

        // PUT: api/LogCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogCategory([FromRoute] int id, [FromBody] LogCategory logCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != logCategory.LogCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(logCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogCategoryExists(id))
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

        // POST: api/LogCategories
        [HttpPost]
        public async Task<IActionResult> PostLogCategory([FromBody] LogCategory logCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.LogCategories.Add(logCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLogCategory", new { id = logCategory.LogCategoryId }, logCategory);
        }

        // DELETE: api/LogCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logCategory = await _context.LogCategories.SingleOrDefaultAsync(m => m.LogCategoryId == id);
            if (logCategory == null)
            {
                return NotFound();
            }

            _context.LogCategories.Remove(logCategory);
            await _context.SaveChangesAsync();

            return Ok(logCategory);
        }

        private bool LogCategoryExists(int id)
        {
            return _context.LogCategories.Any(e => e.LogCategoryId == id);
        }
    }
}