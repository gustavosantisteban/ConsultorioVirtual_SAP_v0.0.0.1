using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.ViewModels
{
    public class FilesSharedViewModel
    {
        public string IdFileSharedRelacion { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        public bool Success { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string IdFile { get; set; }
        public string IdRelacion { get; set; }

    }
}