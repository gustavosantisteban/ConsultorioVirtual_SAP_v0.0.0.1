using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Especialistas")]
    public class Especialista : ApplicationUser
    {
        public Especialista()
        {
            this.Horarios = new List<Horario>();
        }

        public Prefijo Prefijo { get; set; }
        public string ImagenMedico { get; set; }
        public string NumeroMatricula { get; set; }
        public string EspecialidadId { get; set; }
        
        public virtual Especialidad Especialidad { get; set; }
        public virtual ICollection<Turno> Turnos { get; set; }
        public virtual ICollection<Horario> Horarios { get; set; }
        public virtual ICollection<Relacion> RelacionEspecialista { get; set; }
    }

    public enum Prefijo
    {
        Dr = 1,
        Dra = 2,
        Sr = 3,
        Sra = 4
    }
}