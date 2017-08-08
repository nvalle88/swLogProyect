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
        private readonly SwLogDbContext db;

        public LogCategoriesController(SwLogDbContext db)
        {
            this.db = db;
        }

        // GET: api/LogCategorys
        [HttpGet]
        [Route("ListarLogCategories")]
        public List<LogCategory> GetLogCategorys()
        {
            return db.LogCategories.OrderBy(x => x.Name).ToList();
        }

        public Response Existe(LogCategory logCategory)
        {
            var loglevelrespuesta = db.LogCategories.Where(p => p.Name == logCategory.Name).FirstOrDefault();
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
                Resultado = db.LogCategories.Where(p => p.LogCategoryId == logCategory.LogCategoryId).FirstOrDefault(),
            };
        }

        // GET: api/LogCategorys/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logCategory = await db.LogCategories.SingleOrDefaultAsync(m => m.LogCategoryId == id);

            if (logCategory == null)
            {
                return NotFound();
            }

            return Ok(logCategory);
        }

        // PUT: api/LogCategorys/5
        [HttpPut("{id}")]
        [Route("EditarLogCategory")]
        public async Task<Response> PutLogCategory([FromBody] LogCategory logCategory)
        {
            if (!ModelState.IsValid)
            {
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = "Módelo inválido",
                };
            }
            db.Entry(logCategory).State = EntityState.Modified;

            try
            {
                var respuesta = Existe(logCategory);
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
                    Message = "Existe un Log Category de igual Nombre...",
                };
            }
            catch (Exception ex)
            {
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = "Existe un Log Category de igual Nombre...",
                };
            }
        }

        // POST: api/LogCategorys
        [HttpPost]
        [Route("InsertarLogCategory")]

        public async Task<Response> PostLogCategory([FromBody] LogCategory logCategory)
        {
            if (!ModelState.IsValid)
            {
                return new entidades.Response
                {
                    IsSuccess = false,
                    Message = "Módelo inválido",
                };
            }

            try
            {
                var respuesta = Existe(logCategory);
                if (!respuesta.IsSuccess)
                {

                    db.Add(logCategory);
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
                    Message = "Existe un Log Category de igual Nombre...",
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

        // DELETE: api/LogCategorys/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteLogCategory([FromRoute] int id)
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

                var logCategory = await db.LogCategories.SingleOrDefaultAsync(m => m.LogCategoryId == id);
                if (logCategory == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "El Log Category no existe",
                    };
                }

                db.LogCategories.Remove(logCategory);
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

        private bool LogCategoryExists(int id)
        {
            return db.LogCategories.Any(e => e.LogCategoryId == id);
        }
    }
}