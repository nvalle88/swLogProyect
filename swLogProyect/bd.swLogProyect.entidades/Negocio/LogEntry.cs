﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace bd.swLogProyect.entidades
{

    [Table("Log")]
    public class LogEntry
    {
        [Column("LogId")]
        public int LogEntryId { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime LogDate { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Mensaje")]
        public string Message { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Nombre Objeto")]
        public string ObjectName { get; set; }

        [StringLength(4096)]
        [Display(Name = "Excepcion")]
        public string ExceptionTrace { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Nombre Dispositivo")]
        public string MachineName { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "IP Dispositivo")]
        public string MachineIP { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "IP Dispositivo")]
        public string UserName { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "IP Dispositivo")]
        public string ApplicationName { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Objeto Anterior")]
        public string ObjectPrevious { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Objeto Siguiente")]
        public string ObjectNext { get; set; }


        [Required]
        [Column("NivelId")]
        public int LogLevelId { get; set; }

        [Required]
        [Column("CategoriaId")]
        public int LogCategoryId { get; set; }

       

        public virtual LogLevel LogLevel { get; set; }

        public virtual LogCategory LogCategory { get; set; }
    }
}
