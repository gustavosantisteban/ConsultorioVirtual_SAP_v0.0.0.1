using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Calificaciones")]
    public class Calificacion
    {
        public Calificacion()
        {
            this.IdCalificacion = Guid.NewGuid().ToString();
        }

        private string _aprobado;

        [Key]
        public string IdCalificacion { get; set; }
        public string Comentario { get; set; }
        public int calificacion { get; set; }
        public string estadodisputa
        {
            get
            {
                return _aprobado;
            }
            set
            {
                if ((calificacion >= 1) || (calificacion < 4))
                {
                    _aprobado = "disputa";
                } else if((calificacion >= 5) || (calificacion < 7))
                {
                    _aprobado = "bueno";
                } else
                {
                    _aprobado = "aprobada";
                }
            }
        }
        
        public virtual Consulta Consultas { get; set; }
    }
}