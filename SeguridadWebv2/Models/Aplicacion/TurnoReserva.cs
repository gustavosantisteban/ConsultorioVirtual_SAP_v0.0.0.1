using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class TurnoReserva
    {
        public TurnoReserva()
        {
            this.TurnoReservaID = Guid.NewGuid().ToString();
        }
        public string TurnoReservaID { get; set; }
        public string IdTurno { get; set; }
        public string UserId { get; set; }
        public Estado Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("IdTurno")]
        public Turno Turno { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
    }
  
}