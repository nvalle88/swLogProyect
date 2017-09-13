using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swLogProyect.datos;
using bd.swLogProyect.entidades;
using bd.swLogProyect.entidades.ViewModels;
using bd.swLogProyect.entidades.ObjectTranfer;
using bd.swlogProyect.Helpers.Helpers;
using bd.swLogProyect.entidades.Utils;

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

        // GET: api/LogEntries
        [HttpGet]
        public IEnumerable<LogEntry> GetLogEntries()
        {
            return db.LogEntries;
        }

        // GET: api/LogEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogEntry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logEntry = await db.LogEntries.SingleOrDefaultAsync(m => m.LogEntryId == id);

            if (logEntry == null)
            {
                return NotFound();
            }

            return Ok(logEntry);
        }


        // POST: api/LogEntries
        [HttpPost]
        [Route("InsertarLonEntry")]
        public async Task<Response> PostLogEntry([FromBody] LogEntryTranfer logEntryTranfer)
        {
            if (!ModelState.IsValid)
            {
                return new Response
                {
                    IsSuccess=false,
                    Message="MÓDELO INVÁLIDO",
                } ;
            }

            try
            {
                var logLevelID = db.LogLevels.FirstOrDefault(l => l.ShortName.Contains(logEntryTranfer.LogLevelShortName)).LogLevelId;
                var logCategoryID = db.LogCategories.FirstOrDefault(l => l.ParameterValue.Contains(logEntryTranfer.LogCategoryParametre)).LogCategoryId;

                db.Add(new LogEntry
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
                    ObjEntityId = logEntryTranfer.EntityID
                });
               await db.SaveChangesAsync();
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                };

            }
            catch (Exception ex)
            {
              return new  Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
               
            }
        }

        [HttpPost]
        [Route("ListaFiltradaLogEntry")]
        public async Task<List<LogEntry>> GetListaFiltradaLogEntry([FromBody] LogEntryViewModel LogEntryViewModel)
        {
            //return await db.LogEntries.Where(x => x.UserName == "Nestor").ToListAsync();
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

                throw;
            }


        }
    }
}