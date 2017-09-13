using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.swLogProyect.entidades
{
    public enum LogLevelParameter
    {
        ERR, ADV, INFO
    }

    public class LogLevel
    {
        public int LogLevelId { get; set; }

        [Required]
        [Display(Name = "Código")]
        public int Code { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Nombre Corto")]
        public string ShortName { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public virtual ICollection<LogEntry> LogEntries { get; set; }
    }
}
