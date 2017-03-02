using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.ViewModels
{
    public class AnamnesisVM
    {
        public string Motivo { get; set; }
        public string Descripcion { get; set; }
        public bool ValidarMotivo { get; set; }
        public string IdAnamnesis { get; set; }
    }
}