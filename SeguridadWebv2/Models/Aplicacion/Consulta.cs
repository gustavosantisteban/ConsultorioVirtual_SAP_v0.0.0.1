using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Consultas")]
    public class Consulta
    {
        public Consulta()
        {
            this.IdConsulta = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdConsulta { get; set; }
        public DateTime FechaConsulta { get; set; }
        public Estado Estado { get; set; }
        

        public virtual HistoriaClinica HistoriaClinica { get; set; }
        public virtual Turno Turno { get; set; }
        public virtual List<Calificacion> Calificacion { get; set; }
        public virtual Anamnesis Anamnesis { get; set; }
        public virtual List<Receta> Recetas { get; set; }
    }
}