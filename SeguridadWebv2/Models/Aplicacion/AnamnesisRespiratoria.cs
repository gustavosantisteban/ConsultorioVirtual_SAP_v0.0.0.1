using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class AnamnesisRespiratoria : HojaAnamnesisDecorator
    {
        public List<Patologia> PatologiaRespiratoria { get; set; }
    }
}