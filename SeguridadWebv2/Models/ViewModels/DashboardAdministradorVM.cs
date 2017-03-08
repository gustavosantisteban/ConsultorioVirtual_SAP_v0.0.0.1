using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Models.ReportClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Models.ViewModels
{
    public class DashboardAdministradorVM
    {
        public List<Turno> turnosvm { get; set; }
        public List<Paciente> pacientes { get; set; }
        public List<Especialista> especialistas { get; set; }
        public List<OrdenTurno> ordenturno { get; set; }
        public List<SelectListItem> reporteconsultaanual { get; set; }
    }
}