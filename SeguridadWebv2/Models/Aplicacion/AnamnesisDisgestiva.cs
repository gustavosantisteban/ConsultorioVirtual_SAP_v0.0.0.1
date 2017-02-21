using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class AnamnesisDisgestiva : HojaAnamnesisDecorator
    {
        public List<Patologia> PatologiaDigestiva { get; set; }
    }
}