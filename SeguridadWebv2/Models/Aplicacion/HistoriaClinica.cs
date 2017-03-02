using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("HistoriaClinica")]
    public class HistoriaClinica
    {
        public HistoriaClinica()
        {
            this.IdHistoriaClinica = Guid.NewGuid().ToString();
        }
        [Key]
        public string IdHistoriaClinica { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual List<Anamnesis> Anamnesis { get; set; }
        public virtual List<Paciente> Pacientes { get; set; }
    }
}