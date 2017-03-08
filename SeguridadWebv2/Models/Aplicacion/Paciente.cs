﻿using System;
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
            this.Turnos = new List<Turno>();
        }

        public string ImagenPaciente { get; set; }
        public Nullable<DateTime> FechadeCumpleanos { get; set; }
        public Sexo Genero { get; set; }
        public string Direccion { get; set; }
        public string IdObraSocial { get; set; }
        public string IdHistoriaClinica { get; set; }
        [ForeignKey("IdObraSocial")]
        public virtual ObraSocial ObraSocial { get; set; }
        [ForeignKey("IdHistoriaClinica")]
        public virtual HistoriaClinica HistoriaClinica { get; set; }
        public virtual ICollection<Turno> Turnos { get; set; }
        public virtual ICollection<Analisis> Analisis { get; set; }
    }

    public enum Sexo
    {
        Masculino,
        Femenino,
        Otro
    }
}