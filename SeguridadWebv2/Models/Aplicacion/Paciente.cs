using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Pacientes")]
    public class Paciente : ApplicationUser
    {
        public Paciente()
        {
            this.RelacionPaciente = new List<Relacion>();
        }

        public string ImagenPaciente { get; set; }
        public Nullable<DateTime> FechadeCumpleanos { get; set; }
        public Sexo Genero { get; set; }
        public string Direccion { get; set; }
        public string IdObraSocial { get; set; }

        [ForeignKey("IdObraSocial")]
        public virtual ObraSocial ObraSocial { get; set; }
        public virtual ICollection<Relacion> RelacionPaciente { get; set; }
        public virtual ICollection<Analisis> Analisis { get; set; }
    }

    public enum Sexo
    {
        Masculino,
        Femenino,
        Otro
    }
}