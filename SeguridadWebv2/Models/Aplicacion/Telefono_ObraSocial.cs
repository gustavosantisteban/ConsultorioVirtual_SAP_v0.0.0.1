using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Telefono_ObraSocial
    {
        public Telefono_ObraSocial()
        {
            this.IdTelefono_ObraSocial = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdTelefono_ObraSocial { get; set; }
        public string NumeroTelefono { get; set; }
        public virtual ObraSocial ObraSocial { get; set; }
    }
}