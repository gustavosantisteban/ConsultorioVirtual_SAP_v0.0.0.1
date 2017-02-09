using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class File
    {
        public File()
        {
            this.IdFile = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdFile { get; set; }
        public string FullPath { get; set; }
        public DateTime Fecha { get; set; }
        public string IdUsuario { get; set; }
        public bool Estado { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual ApplicationUser Usuario { get; set; }
    }
}