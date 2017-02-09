using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("ObrasSociales")]
    public class ObraSocial
    {
        public ObraSocial()
        {
            this.IdObraSocial = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdObraSocial { get; set; }
        public string RazonSocial { get; set; }
        public string PaginaWeb { get; set; }

        public virtual ICollection<Contacto_ObraSocial> ContactoObraSociales { get; set; }
        public virtual ICollection<Direccion_ObraSocial> DireccionObraSociales { get; set; }
        public virtual ICollection<Telefono_ObraSocial> TelefonoObraSociales { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}