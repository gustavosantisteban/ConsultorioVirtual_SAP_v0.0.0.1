using SeguridadWebv2.Models.Aplicacion;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeguridadWebv2.Models.ViewModels
{
    public class TurnosViewModel
    {
        public string IdTurno { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Dia { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? HoraInicio { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:hh:mm}")]
        public DateTime? HoraFin { get; set; }
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