using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Diagnostico
    {
        public Diagnostico()
        {
            this.Id_Diagnostico = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id_Diagnostico { get; set; }
        public string IdPaciente { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaDiagnostico { get; set; }
    }
}