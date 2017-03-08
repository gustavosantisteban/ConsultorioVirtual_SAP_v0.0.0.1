using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.ViewModels
{
    public class ConsultaViewModel
    {
        public HistoriaClinica HistoriaClinica { get; set; }
        public TurnosViewModel TurnoVM { get; set; }
        public List<Calificacion> Calificacion { get; set; }
        public Anamnesis Anamnesis { get; set; }
        public List<Receta> Recetas { get; set; }
    }
}