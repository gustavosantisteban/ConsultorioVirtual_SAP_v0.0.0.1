using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Pregunta
    {
        public Pregunta()
        {
            this.IDPregunta = Guid.NewGuid().ToString();
        }

        [Key]
        public string IDPregunta { get; set; }
        public string Descripcion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime FechaPregunta { get; set; }
        public Estado EstadoPregunta { get; set; }
    }
}