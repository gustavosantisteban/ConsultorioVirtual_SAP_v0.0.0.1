using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Recetas")]
    public class Receta
    {
        public Receta()
        {
            this.IdReceta = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdReceta { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fechareceta { get; set; }

        public virtual Consulta Consulta { get; set; }
    }
}