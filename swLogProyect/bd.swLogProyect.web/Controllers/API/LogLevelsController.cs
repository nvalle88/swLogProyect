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
        private readonly SwLogDbContext db;

        public LogLevelsController(SwLogDbContext db)
        {
            this.db = db;
        }

        // GET: api/LogLevels
        [HttpGet]
        [Route("ListarLogLevels")]
        public List<LogLevel> GetLogLevels()
        {
            return db.LogLevels.OrderBy(x=>x.ShortName).ToList();
        }

        public Response Existe(LogLevel logLevel)
        {
            var loglevelrespuesta = db.LogLevels.Where(p => p.Name == logLevel.Name).FirstOrDefault();
            if (loglevelrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Existe una categoría de igual nombre",
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Message = "No existe país...",
                Resultado = db.LogLevels.Where(p => p.LogLevelId == logLevel.LogLevelId).FirstOrDefault(),
            };
        }

        // GET: api/LogLevels/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogLevel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logLevel = await db.LogLevels.SingleOrDefaultAsync(m => m.LogLevelId == id);

            if (logLevel == null)
            {
                return NotFound();
            }

            return Ok(logLevel);
        }

        // PUT: api/LogLevels/5
        [HttpPut("{id}")]
        [Route("EditarLogLevel")]
        public async Task<Response> PutLogLevel( [FromBody] LogLevel logLevel)
        {
            if (!ModelState.IsValid)
            {
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = "Módelo inválido",
                };
            }
            db.Entry(logLevel).State = EntityState.Modified;

            try
            {
                var respuesta = Existe(logLevel);
                if (!respuesta.IsSuccess)
                {
                     await db.SaveChangesAsync();
                     return new entidades.Response
                    {
                        IsSuccess = true,
                        Message = "Ok",
                    };
                }
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = "Existe un Log Level de igual Nombre...",
                };
            }
            catch (Exception ex)
            {
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = "Existe un Log Level de igual Nombre...",
                };
            }
        }

        // POST: api/LogLevels
        [HttpPost]
        [Route("InsertarLogLevel")]

        public async Task<Response> PostLogLevel([FromBody] LogLevel logLevel)
        {
            if (!ModelState.IsValid)
            {
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message="Módelo inválido",
                };
            }

            try
            {
                var respuesta = Existe(logLevel);
                if (!respuesta.IsSuccess)
                {

                    db.Add(logLevel);
                    await db.SaveChangesAsync();
                    return new entidades.Response
                    {
                        IsSuccess = true,
                        Message = "Ok",
                    };
                }

                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = "Existe un Log Level de igual Nombre...",
                };

            }
            catch (Exception ex)
            {
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }

           
        }

        // DELETE: api/LogLevels/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteLogLevel([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Módelo no válido",
                    };
                }

                var logLevel = await db.LogLevels.SingleOrDefaultAsync(m => m.LogLevelId == id);
                if (logLevel == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "El Log Level no existe",
                    };
                }

                db.LogLevels.Remove(logLevel);
                await db.SaveChangesAsync();
                return new Response
                {
                    IsSuccess = true,
                    Message = "Se ha eliminado ",
                };

            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = ex.Message,
                };
             
            }
        }

        private bool LogLevelExists(int id)
        {
            return db.LogLevels.Any(e => e.LogLevelId == id);
        }
    }
}