using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.ReportClass
{
    [Table("ReporteConsulta")]
    public class ReporteConsultaAnual
    {
        public ReporteConsultaAnual()
        {
            this.IdReporteConsulta = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdReporteConsulta { get; set; }
        public DateTime MesReporte { get; set; }
        public int CantidadConsultas20M { get; set; }
        public int CantidadConsultas30M { get; set; }
    }
}