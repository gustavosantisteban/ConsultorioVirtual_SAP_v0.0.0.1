using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models
{
    public class SearchViewModel
    {
        [Required]
        [Display(Name = "Ingrese una Especialidad")]
        public string Especialidad { get; set; }
    }
}