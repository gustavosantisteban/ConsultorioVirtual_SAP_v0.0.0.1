using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Patologia
    {
        public Patologia()
        {
            this.IdPatologia = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdPatologia { get; set; }
        public string Motivo { get; set; }
        public string Descripcion { get; set; }
        public bool ValidarMotivo { get; set; }
    }
}