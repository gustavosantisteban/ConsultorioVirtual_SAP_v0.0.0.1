using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Horario
    {
        public Horario()
        {
            this.IDHorario = Guid.NewGuid().ToString();
            this.HorariosDisponibles = new List<HorarioDisponible>();
        }

        [Key]
        public string IDHorario { get; set; }
        public int Duracion { get; set; }
        public EstadoHorario Estado { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        public DateTime HoraInicio { get; set; }
        public int CantidadTurnos { get; set; }
        public string EspecialistaId { get; set; }

        [ForeignKey("EspecialistaId")]
        public virtual Especialista Especialista { get; set; }
        public DiasDelaSemana Dia { get; set; }
        public MesDelAno Mes { get; set; }
        public virtual List<HorarioDisponible> HorariosDisponibles { get; set; }
    }

    public enum EstadoHorario
    {
        Disponible = 1,
        Ocupado = 2,
        Pendiente = 3
    }
    
    public enum DiasDelaSemana
    {
        Domingo = 0,
        Lunes = 1,
        Martes = 2,
        Miercoles = 3,
        Jueves = 4,
        Viernes = 5,
        Sabado = 6
    }

    public enum MesDelAno
    {
        Enero = 1,
        Febrero = 2,
        Marzo = 3,
        Abril = 4,
        Mayo = 5,
        Junio = 6,
        Julio = 7,
        Agosto = 8,
        Septiembre = 9,
        Octubre = 10,
        Noviembre = 11,
        Diciembre = 12
    }
}