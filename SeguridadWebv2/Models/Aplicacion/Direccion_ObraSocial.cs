using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Direccion_ObraSocial
    {
        public Direccion_ObraSocial()
        {
            this.IdDireccion_ObraSocial = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdDireccion_ObraSocial { get; set; }
        public string Direccion { get; set; }
        public string NumeroDireccion { get; set; }
        public virtual ObraSocial ObraSocial { get; set; }
    }
}