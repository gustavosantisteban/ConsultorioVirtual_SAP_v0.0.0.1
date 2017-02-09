using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Servicio
    {
        public Servicio()
        {
            this.ServicioId = Guid.NewGuid().ToString();
        }

        [Key]
        public string ServicioId { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
    }
}