using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("OrdenTurno")]
    public class OrdenTurno
    {
        public OrdenTurno()
        {
            this.IdOrdenTurno = Guid.NewGuid().ToString();
            ordenitems = new List<OrdenItem>();
        }

        [Key]
        public string IdOrdenTurno { get; set; }
        public string SessionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Status { get; set; }
        public string MPRefID { get; set; }
        public string MPCollectionID { get; set; }

        public virtual List<OrdenItem> ordenitems { get; set; }
    }
}