using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class HorarioDisponible
    {
        public HorarioDisponible()
        {
            this.Id = Guid.NewGuid().ToString();
            Disponible = EstadoHorario.Disponible;
        }
        [Key]
        public string Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Time)]
        public DateTime Dia { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        public DateTime HoraInicio { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        public DateTime HoraFin { get; set; }        
        public EstadoHorario Disponible { get; set; }
        public string HorarioId { get; set; }
        public decimal Precio { get; set; }

        [ForeignKey("HorarioId")]
        public virtual Horario Horario { get; set; }
    }
    
}