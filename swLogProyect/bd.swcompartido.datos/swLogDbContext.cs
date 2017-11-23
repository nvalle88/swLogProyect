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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<bd.swLogProyect.entidades.LogCategory>(entity =>
            {
                entity.HasKey(e => e.LogCategoryId)
                    .HasName("PK_LogCategories");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.ParameterValue)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<bd.swLogProyect.entidades.LogEntry>(entity =>
            {
                entity.HasKey(e => e.LogEntryId)
                    .HasName("PK_LogEntries");

                entity.HasIndex(e => e.LogCategoryId)
                    .HasName("IX_LogEntries_LogCategoryId");

                entity.HasIndex(e => e.LogLevelId)
                    .HasName("IX_LogEntries_LogLevelId");

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.MachineIP)
                    .IsRequired()
                    .HasColumnName("MachineIP")
                    .HasMaxLength(1024);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.ObjectName)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.ObjectNext).HasMaxLength(1024);

                entity.Property(e => e.ObjectPrevious).HasMaxLength(1024);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(1024);
            });

            modelBuilder.Entity<bd.swLogProyect.entidades.LogLevel>(entity =>
            {
                entity.HasKey(e => e.LogLevelId)
                    .HasName("PK_LogLevels");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ShortName)
                    .IsRequired()
                    .HasMaxLength(5);
            });
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
        }

    }

}






