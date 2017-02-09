using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Contacto_ObraSocial
    {
        public Contacto_ObraSocial()
        {
            this.IdContacto_ObraSocial = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdContacto_ObraSocial { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public virtual ObraSocial ObraSocial { get; set; }
    }
}