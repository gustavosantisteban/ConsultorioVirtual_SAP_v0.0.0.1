using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.ViewModels
{
    public class TurnosViewModel
    {
        public string IdTurno { get; set; }
        public DateTime Dia { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public Estado EstadoTurno { get; set; }
        public string RelacionId { get; set; }
        public decimal Precio { get; set; }
        public Especialista Especialista { get; set; }
        public Paciente Paciente { get; set; }

        public string FullNamePacienteTurno {
            get { return this.Paciente.Nombre + " " + this.Paciente.Apellido; }
        }

        public string FullNameEspecialistaTurno
        {
            get { return this.Especialista.Nombre + " " + this.Especialista.Apellido; }
        }

    }
}