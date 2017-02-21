using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class HistoriaClinica
    {
        public HistoriaClinica()
        {
            this.IdHistoriaClinica = Guid.NewGuid().ToString();
        }
        [Key]
        public string IdHistoriaClinica { get; set; }
        public List<Anamnesis> FichaMedica { get; set; }
        public DateTime FechaCreacion { get; set; }

        public virtual Paciente Paciente { get; set; }
    }
}