using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.ViewModels
{
    public class HorarioViewModel
    {
        public HorarioViewModel()
        {
            this.HorariosDisponibles = new List<HorarioDisponible>();
        }

        public string IDHorario { get; set; }
        public int Duracion { get; set; }
        public EstadoHorario Estado { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        public DateTime HoraInicio { get; set; }
        public int CantidadTurnos { get; set; }
        public DiasDelaSemana Dia { get; set; }
        public MesDelAno Mes { get; set; }
        public Especialista Especialista { get; set; }
        public List<HorarioDisponible> HorariosDisponibles { get; set; }
        public string EspecialistaID { get; set; }

        public string FullNombreEspecialista
        {
            get
            {
                return this.Especialista.Nombre + " " + this.Especialista.Apellido;
            }
        }
    }
}