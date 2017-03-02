using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("OrdenItem")]
    public class OrdenItem
    {
        public OrdenItem()
        {
            this.IdOrdenItem = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdOrdenItem { get; set; }
        public string title { get; set; }
        public string currency_id { get; set; }
        public decimal unit_price { get; set; }
        public int quantity { get; set; }
        public bool EsValido { get; set; }
        public string IdHorarioDisponible { get; set; }
        public string IdOrdenTurno { get; set; }
        
        [ForeignKey("IdHorarioDisponible")]
        public virtual HorarioDisponible HorarioDisponible { get; set; }
        [ForeignKey("IdOrdenTurno")]
        public virtual OrdenTurno OrdenTurno { get; set; }
    }
}