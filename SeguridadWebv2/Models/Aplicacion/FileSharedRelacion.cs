using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class FileSharedRelacion
    {
        public FileSharedRelacion()
        {
            this.IdFileSharedRelacion = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdFileSharedRelacion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        public bool Success { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string IdFile { get; set; }
        public string IdRelacion { get; set; }

        [ForeignKey("IdFile")]
        public virtual File File { get; set; }

    }
}