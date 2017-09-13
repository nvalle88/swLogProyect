using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swLogProyect.datos;
using bd.swLogProyect.entidades;
using bd.swLogProyect.entidades.Utils;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swLogProyect.entidades.Enumeradores;

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
        public async Task<List<LogLevel>> GetLogLevels()
        {
            try
            {
                return await db.LogLevels.OrderBy(x => x.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.Logs),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepción",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<LogLevel>();
            }
        }

        // GET: api/LogLevels/5
        [HttpGet("{id}")]
        public async Task<Response> GetLogLevel([FromRoute] int id)
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

                var loglevel = await db.LogLevels.SingleOrDefaultAsync(m => m.LogLevelId == id);

                if (loglevel == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No encontrado",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Resultado = loglevel,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.Logs),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepción",
                    LogCategoryParametre= Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }



        // PUT: api/LogLevels/5
        [HttpPut("{id}")]
        public async Task<Response> PutLogLevel([FromRoute] int id, [FromBody] LogLevel LogLevel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Módelo inválido"
                    };
                }

                var LogLevelActualizar = await db.LogLevels.Where(x => x.LogLevelId == id).FirstOrDefaultAsync();
                if (LogLevelActualizar != null)
                {
                    try
                    {
                        LogLevelActualizar.Description = LogLevel.Description;
                        LogLevelActualizar.Name = LogLevel.Name;
                        LogLevelActualizar.Code = LogLevel.Code;
                        LogLevelActualizar.ShortName = LogLevel.ShortName;
                        db.LogLevels.Update(LogLevelActualizar);
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Ok",
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.Logs),
                            ExceptionTrace = ex,
                            Message = "Se ha producido una excepción",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Error ",
                        };
                    }
                }




                return new Response
                {
                    IsSuccess = false,
                    Message = "Existe"
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Excepción"
                };
            }
        }

        // POST: api/LogLevels
        [HttpPost]
        [Route("InsertarLogLevel")]
        public async Task<Response> PostLogLevel([FromBody] LogLevel LogLevel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Módelo inválido"
                    };
                }

                var respuesta = Existe(LogLevel);
                if (!respuesta.IsSuccess)
                {
                    db.LogLevels.Add(LogLevel);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "OK"
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = "OK"
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.Logs),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una exepción",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
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
                        Message = "Módelo no válido ",
                    };
                }

                var respuesta = await db.LogLevels.SingleOrDefaultAsync(m => m.LogLevelId == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No existe ",
                    };
                }
                db.LogLevels.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = "Eliminado ",
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.Logs),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepción",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        private bool LogLevelExists(int id)
        {
            return db.LogLevels.Any(e => e.LogLevelId == id);
        }

        public Response Existe(LogLevel LogLevel)
        {
            var bdd = LogLevel.ShortName.ToUpper().TrimEnd().TrimStart();
            var loglevelrespuesta = db.LogLevels.Where(p => p.ShortName.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            if (loglevelrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Existe un log level de igual nombre",
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = loglevelrespuesta,
            };
        }

    }
}