using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class AnamnesisCardiovascular : HojaAnamnesisDecorator
    {
        public string Encabezado { get; set; }
        public string Texto { get; set; }
        
        public virtual HistoriaClinica HistoriaClinica { get; set; }
    }
}