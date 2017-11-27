using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swLogProyect.datos;
using bd.swLogProyect.entidades;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swLogProyect.entidades.Enumeradores;
using bd.swLogProyect.entidades.Utils;
using bd.swlogProyect.Helpers.Helpers;
using bd.swLogProyect.entidades.ViewModels;

namespace bd.swLogProyect.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/LogEntries")]
    public class LogEntriesController : Controller
    {
        private readonly SwLogDbContext db;

        public LogEntriesController(SwLogDbContext db)
        {
            this.db = db;
        }


        // POST: api/LogEntries
        [HttpPost]
        [Route("InsertarLonEntry")]
        public async Task<Response> PostLogEntry([FromBody] LogEntryTranfer logEntryTranfer)
        {

            string objectNext = string.Empty, objectPrevious = string.Empty;

            if (!ModelState.IsValid)
            {
                return new Response
                {
                    IsSuccess=false,
                    Message=Mensaje.ModeloInvalido,
                } ;
            }



            if (logEntryTranfer.ObjectNext == null)
            {
                objectNext = "NULL";
            }
            else
            {
                objectNext = logEntryTranfer.ObjectNext.ToString();
            }

            if (logEntryTranfer.ObjectPrevious == null)
            {
                objectPrevious = "NULL";
            }
            else
            {
                objectPrevious = logEntryTranfer.ObjectPrevious.ToString();
            }
            try
            {
                var logLevelID = db.LogLevels.FirstOrDefault(l => l.ShortName.Contains(logEntryTranfer.LogLevelShortName)).LogLevelId;
                var logCategoryID = db.LogCategories.FirstOrDefault(l => l.ParameterValue.Contains(logEntryTranfer.LogCategoryParametre)).LogCategoryId;

                

                LogEntry logEntry = new LogEntry
                {
                    UserName = logEntryTranfer.UserName,
                    ApplicationName = logEntryTranfer.ApplicationName,
                    ExceptionTrace = (logEntryTranfer.ExceptionTrace != null) ? LogEntryHelper.GetAllErrorMsq(logEntryTranfer.ExceptionTrace) : null,
                    LogCategoryId = logCategoryID,
                    LogLevelId = logLevelID,
                    LogDate = DateTime.Now,
                    MachineIP = LogNetworkHelper.GetRemoteIpClientAddress(),
                    MachineName = LogNetworkHelper.GetClientMachineName(),
                    Message = logEntryTranfer.Message,
                    ObjectName = logEntryTranfer.EntityID,
                    ObjectPrevious = objectPrevious,
                    ObjectNext = objectNext
                };



                db.Add(logEntry);
               await db.SaveChangesAsync();
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.Logs),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };

            }
        }


        // GET: api/LogEntries/5
        [HttpGet("{id}")]
        public async Task<Response> GetLogEntries([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var logentry = await db.LogEntries.SingleOrDefaultAsync(m => m.LogEntryId == id);

                if (logentry == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = logentry,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.Logs),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        [HttpPost]
        [Route("ListaFiltradaLogEntry")]
        public async Task<List<LogEntry>> GetListaFiltradaLogEntry([FromBody] LogEntryViewModel LogEntryViewModel)
        {
            var DateStart = LogEntryViewModel.LogDateStart.HasValue ? LogEntryViewModel.LogDateStart : null;
            var DateFinish = LogEntryViewModel.LogDateFinish.HasValue ? LogEntryViewModel.LogDateFinish : null;

            try
            {
                return await ((db.LogEntries.
                    Where(x => (x.LogLevel.LogLevelId == LogEntryViewModel.LogLevelId || LogEntryViewModel.LogLevelId == 0)
                    && (x.LogCategoryId == LogEntryViewModel.LogCategoryId || LogEntryViewModel.LogCategoryId == 0)
                    && (x.ApplicationName.Contains(LogEntryViewModel.ApplicationName) || LogEntryViewModel.ApplicationName == null)
                    && (x.MachineIP.Contains(LogEntryViewModel.MachineIP) || LogEntryViewModel.MachineIP == null)
                    && (x.UserName.Contains(LogEntryViewModel.UserName) || LogEntryViewModel.UserName == null)
                    && (x.LogDate.Date >= DateStart || DateStart == null)
                    && (x.LogDate.Date <= DateFinish|| DateFinish == null)
                    && (x.MachineName.Contains(LogEntryViewModel.MachineName) || LogEntryViewModel.MachineName == null))).OrderByDescending(x => x.LogDate).ToListAsync());
            }
            catch (Exception ex)
            {

                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.Logs),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<LogEntry>();
            }


        }
    }
}