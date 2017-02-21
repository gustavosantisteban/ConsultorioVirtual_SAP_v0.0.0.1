using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class AnamnesisCardiovascular : HojaAnamnesisDecorator
    {
        public List<Patologia> PatologiaCardiovascular { get; set; }
    }
}