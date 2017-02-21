using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class MotivoConsulta
    {
        public MotivoConsulta()
        {
            this.IdMotivoConsulta = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdMotivoConsulta { get; set; }
        public string Motivo { get; set; }
        public string Descripcion { get; set; }
        public bool ValidarMotivo { get; set; }

        public virtual Anamnesis Anamnesis { get; set; }
    }
}