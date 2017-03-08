using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public abstract class Anamnesis
    {
        public Anamnesis()
        {
            this.IdAnamnesis = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdAnamnesis { get; set; }
        public DateTime Fecha { get; set; }
        //public List<MotivoConsulta> MotivoConsulta { get; set; }
        public virtual List<Consulta> Consultas { get; set; }
        
    }
}