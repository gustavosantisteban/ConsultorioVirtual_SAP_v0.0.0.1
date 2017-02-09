using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Analisis")]
    public class Analisis
    {
        public Analisis()
        {
            this.IdAnalisis = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdAnalisis { get; set; }
        public DateTime FechaAnalisis { get; set; }

        public virtual Especialista Especialista { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}