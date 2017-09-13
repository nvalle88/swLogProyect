using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace bd.swLogProyect.entidades
{
    public enum LogCategoryParameter
    {
        Permission, Activation, Critical, ChangeInformation, NetActivity, Create, Edit, Delete
    }

    public class LogCategory
    {
        public int LogCategoryId { get; set; }

        [Required]
        [StringLength(32)]
        [Display(Name = "Parámetro")]
        public string ParameterValue { get; set; }

        [Required]
        [StringLength(64)]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Descripción")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ICollection<LogEntry> LogEntries { get; set; }

    }
}
