using Microsoft.EntityFrameworkCore;
using System.Linq;
using bd.swLogProyect.entidades;
using System.Collections.Generic;
using System;

namespace bd.swLogProyect.datos
{
    public class SwLogDbContext : DbContext
    {

        public SwLogDbContext(DbContextOptions<SwLogDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {


            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }


        public DbSet<bd.swLogProyect.entidades.LogCategory> LogCategories { get; set; }
        public DbSet<bd.swLogProyect.entidades.LogLevel> LogLevels { get; set; }
        public DbSet<bd.swLogProyect.entidades.LogEntry> LogEntries { get; set; }


        public void EnsureSeedData()
        {
            if (!this.LogCategories.Any())
            {
                var logCategories = new List<LogCategory>
                    {
                        new LogCategory
                        {
                             Name = "Autenticación y Autorización"
                             ,Description = "Esta categoría permite identificar los intentos exitosos y fallidos de acceso al aplicativo (autenticación), así como las actividades ejecutadas por el usuario de acuerdo a sus privilegios dentro del aplicativo (autorización)." +
                             "Los principales registros de esta categoría son:" +
                             "1.- Todos los intentos de acceso exitosos o fallidos por usuario." +
                             "2.- Todos los accesos fuera de las horas de trabajo." +
                             "3.- Múltiples accesos de una misma cuenta desde diferentes dispositivos." +
                             "4.- Múltiples intentos de acceso fallidos seguido por un acceso exitoso desde la misma cuenta."
                             ,ParameterValue = "Permission"
                        },

                        new LogCategory
                        {
                             Name = "Activación"
                             ,Description = "Esta categoría se refiere a la acción de Activar/Desactivar un registro correspondiente a una entidad."
                             ,ParameterValue = "Activation"
                        },

                        new LogCategory
                        {
                             Name = "Errores Críticos y Fallos del Aplicativo"
                             ,Description = "Estos registros se refieren a errores o fallos cuya incidencia afecta el correcto desempeño del aplicativo, los cuales colateralmente pueden afectar la seguridad." +
                             "En esta categoría entran los siguientes registros:" +
                             "1.- Errores críticos." +
                             "2.- Caídas del aplicativo, cierres y reinicios." +
                             "3.- Fallos en la salva y restaura de la información." +
                             "4.- Copias no autorizadas de información."
                             ,ParameterValue = "Critical"
                        },

                        new LogCategory
                        {
                             Name = "Cambios de Información Relevante"
                             ,Description = "Esta categoría se refiere a cambios de información relevante o sensible para el aplicativo." +
                             "En esta categoría entran los siguientes registros:" +
                             "1.- Creación, actualización o eliminación de cuentas de usuario o grupos de usuarios." +
                             "2.- Creación de nuevas cuentas administrativas." +
                             "3.- Cambios de contraseña de usuarios." +
                             "4.- Cambios en la configuración de provincias, municipios, estudios, etc."
                             ,ParameterValue = "ChangeInformation"
                        },

                        new LogCategory
                        {
                             Name = "Actividad de Redes"
                             ,Description = "Esta categoría identifica a los registros referidos a actividades en la red." +
                             "En esta categoría entran los siguientes registros:" +
                             "1.- Todas las conexiones remotas al aplicativo." +
                             "2.- Todas las conexiones internas fuera del horario laboral." +
                             "3.- Transferencia de largos volúmenes de información."
                             ,ParameterValue = "NetActivity"
                        },

                        new LogCategory
                        {
                             Name = "Creación de Registro"
                             ,Description = "Esta categoría identifica a los registros referidos al proceso de creación dentro del CRUD"
                             ,ParameterValue = "Create"
                        },

                        new LogCategory
                        {
                             Name = "Edición de Registro"
                             ,Description = "Esta categoría identifica los registros referidos a la modificación dentro del CRUD"
                             ,ParameterValue = "Edit"
                        },

                        new LogCategory
                        {
                             Name = "Borrado de Registro"
                             ,Description = "Esta categoría identifica los registros referidos al borrado dentro del CRUD"
                             ,ParameterValue = "Delete"
                        }

                    };
                this.AddRange(logCategories);
                this.SaveChanges();
            }

            if (!this.LogLevels.Any())
            {
                var logLevels = new List<LogLevel>
                    {
                        new LogLevel
                        {
                            Code = 100
                            ,Name = "Error"
                            ,ShortName = "ERR"
                            ,Description = "Este nivel se aplica a los registros vinculados a problemas significativos dentro del aplicativo, como perdida de datos o perdida de funcionalidad."
                        },

                        new LogLevel
                        {
                            Code = 200
                            ,Name = "Advertencia"
                            ,ShortName = "ADV"
                            ,Description = "Los registros de este nivel indican problemas no necesariamente significativos, pero que pueden indicar posibles problemas futuros. Si el aplicativo puede recuperarse sin perdidas de datos o funcionalidades entonces el registro califica bajo este nivel."
                        },

                        new LogLevel
                        {
                            Code = 300
                            ,Name = "Información"
                            ,ShortName = "INFO"
                            ,Description = "Este nivel se aplica a los registros que describen operaciones exitosas que deben ser registradas por el aplicativo."
                        }

                    };
                this.AddRange(logLevels);
                this.SaveChanges();
            }

            if (!this.LogEntries.Any())
            {
                new LogEntry
                {
                    ApplicationName = "Aplicación 1",
                    ExceptionTrace = ",",
                    LogCategoryId = 1,
                    LogDate = DateTime.Now,
                    LogLevelId = 1,
                    MachineIP = "10.10.10.10",
                    MachineName = "PC1",
                    Message = "Objeto prueba 1",
                    ObjEntityId = "1",
                    UserName = "Nestor"
                };

                for (int i = 0; i < 30000; i++)
                {
                    if (i < 1000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación1",
                            ExceptionTrace = ",",
                            LogCategoryId = 1,
                            LogDate = DateTime.MinValue,
                            LogLevelId = 1,
                            MachineIP = "10.10.10.10",
                            MachineName = "PC1",
                            Message = "Objeto prueba 1",
                            ObjEntityId = "1",
                            UserName = "Nestor"
                        });
                    }

                    if (i >= 1000 && i < 2000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación2",
                            ExceptionTrace = ",",
                            LogCategoryId = 2,
                            LogDate = DateTime.Now,
                            LogLevelId = 2,
                            MachineIP = "10.10.10.20",
                            MachineName = "PC2",
                            Message = "Objeto prueba 2",
                            ObjEntityId = "2",
                            UserName = "Nestor 2"
                        });
                    }

                    if (i >= 2000 && i < 3000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación3",
                            ExceptionTrace = ",",
                            LogCategoryId = 1,
                            LogDate = DateTime.MaxValue,
                            LogLevelId = 1,
                            MachineIP = "10.10.10.30",
                            MachineName = "PC1",
                            Message = "Objeto prueba 3",
                            ObjEntityId = "1",
                            UserName = "Nestor 3"
                        });
                    }

                    if (i >= 3000 && i < 4000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación4",
                            ExceptionTrace = ",",
                            LogCategoryId = 1,
                            LogDate = DateTime.Now,
                            LogLevelId = 1,
                            MachineIP = "10.10.10.40",
                            MachineName = "PC4",
                            Message = "Objeto prueba 4",
                            ObjEntityId = "4",
                            UserName = "Nestor 4"
                        });
                    }

                    if (i >= 4000 && i <5000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación5",
                            ExceptionTrace = ",",
                            LogCategoryId = 1,
                            LogDate = DateTime.Now,
                            LogLevelId = 1,
                            MachineIP = "10.10.10.50",
                            MachineName = "PC5",
                            Message = "Objeto prueba 5",
                            ObjEntityId = "1",
                            UserName = "Nestor"
                        });
                    }

                    if (i >= 5000 && i < 6000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación6",
                            ExceptionTrace = ",",
                            LogCategoryId = 1,
                            LogDate = DateTime.Now,
                            LogLevelId = 1,
                            MachineIP = "10.10.10.60",
                            MachineName = "PC6",
                            Message = "Objeto prueba 6",
                            ObjEntityId = "6",
                            UserName = "Nestor 6"
                        });
                    }

                    /////

                    if (i >= 7000 && i < 8000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación1",
                            ExceptionTrace = ",",
                            LogCategoryId = 2,
                            LogDate = DateTime.Now,
                            LogLevelId = 2,
                            MachineIP = "10.10.10.70",
                            MachineName = "PC7",
                            Message = "Objeto prueba 7",
                            ObjEntityId = "7",
                            UserName = "Nestor"
                        });
                    }

                    if (i >= 8000 && i < 9000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación8",
                            ExceptionTrace = ",",
                            LogCategoryId = 2,
                            LogDate = DateTime.Now,
                            LogLevelId = 2,
                            MachineIP = "10.10.10.80",
                            MachineName = "PC8",
                            Message = "Objeto prueba 8",
                            ObjEntityId = "1",
                            UserName = "Nestor 8"
                        });
                    }

                    if (i >= 9000 && i < 10000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación9",
                            ExceptionTrace = ",",
                            LogCategoryId = 2,
                            LogDate = DateTime.Now,
                            LogLevelId = 2,
                            MachineIP = "10.10.10.90",
                            MachineName = "PC4",
                            Message = "Objeto prueba 9",
                            ObjEntityId = "9",
                            UserName = "Nestor 9"
                        });
                    }

                    if (i >= 10000 && i < 11000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación10",
                            ExceptionTrace = ",",
                            LogCategoryId = 2,
                            LogDate = DateTime.Now,
                            LogLevelId = 2,
                            MachineIP = "10.10.10.100",
                            MachineName = "PC5",
                            Message = "Objeto prueba 10",
                            ObjEntityId = "1",
                            UserName = "Nestor 10"
                        });
                    }

                    if (i >= 12000 && i < 13000)
                    {

                        this.LogEntries.Add(new LogEntry
                        {
                            ApplicationName = "Aplicación11",
                            ExceptionTrace = ",",
                            LogCategoryId = 2,
                            LogDate = DateTime.Now,
                            LogLevelId = 2,
                            MachineIP = "10.10.10.110",
                            MachineName = "PC6",
                            Message = "Objeto prueba 11",
                            ObjEntityId = "11",
                            UserName = "Nestor 11"
                        });
                    }
                    this.SaveChangesAsync();
                }

            }
        }

    }

}






