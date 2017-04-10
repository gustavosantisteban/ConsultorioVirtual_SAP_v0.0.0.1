using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net;
using SeguridadWebv2.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.ReportClass;
using Newtonsoft.Json;

namespace SeguridadWebv2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<Especialidad> especialidades = db.Especialidades.ToList();
            return View(especialidades.Take(6));
        }

        [HttpGet]
        public ActionResult BuscarEspecialidades(string busqueda)
        {
            busqueda = busqueda.ToUpper().Trim();
            List<string> especialidades = ObtenerEspecialidades().Where(x => x.NombreEspecialidad.ToUpper().Contains(busqueda)).Select(x => x.NombreEspecialidad).ToList();

            return Json(especialidades, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<Especialidad> ObtenerEspecialidades()
        {
            List<Especialidad> especialidades = db.Especialidades.ToList();
            return especialidades;
        }

        public IEnumerable<Especialista> ObtenerEspecialistas()
        {
            List<Especialista> especialistas = db.Especialistas.ToList();
            return especialistas;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Dashboard()
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var rol = UserManager.GetRoles(usuario);
            if (rol.Contains("Paciente") || rol.Contains("AllPacientes"))
            {
                var viewmodel = this.DashboardPaciente(usuario);
                return View(viewmodel);
            }
            if (rol.Contains("Profesionales") || rol.Contains("AllProfesionales"))
            {
                var viewmodel = this.DashboardEspecialista(usuario);
                return View(viewmodel);
            }
            if (rol.Contains("Admin")) {
                var viewmodel = this.DashboardAdministrator();
                return View(viewmodel);
            }
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> MostrarProfesionales(string especialidad)
        {
            if (String.IsNullOrEmpty(especialidad))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad id = db.Especialidades.FirstOrDefault(x => x.NombreEspecialidad.Contains(especialidad.ToUpper()));

            IQueryable<Especialista> esp = db.Especialistas.Where(x => x.Especialidad.EspecialidadId.Contains(id.EspecialidadId));

            var viewmodel = new PreguntasEspecialistaViewModel()
            {
                Especialistas = await esp.ToListAsync(),
                PreguntasVm = new PreguntaViewModel()
            };

            return View(viewmodel);
        }


        [HttpGet]
        [Authorize]
        public DashboardPacienteVM DashboardPaciente(string PacienteID)
        {
            IEnumerable<Turno> turnospacientes = (from turnos in db.Turnos
                                                  join paciente in db.Pacientes on turnos.Paciente.Id equals paciente.Id
                                                  where turnos.Paciente.Id == PacienteID && turnos.EstadoTurno == Estado.Pendiente
                                                  select turnos).ToList();


            ICollection<TurnosViewModel> _turnosvm = turnospacientes.Select(b => new TurnosViewModel
            {
                IdTurno = b.IdTurno,
                Dia = b.Dia.Date,
                HoraInicio = b.HoraInicio,
                HoraFin = b.HoraFin,
                Precio = b.Precio,
                EstadoTurno = b.EstadoTurno,
                Especialista = b.Especialista
            }).ToList();

            var viewmodel = new DashboardPacienteVM()
            {
                TurnosViewModel = _turnosvm
            };
            return viewmodel;
        }


        [HttpGet]
        [Authorize]
        public DashboardEspecialistaVM DashboardEspecialista(string EspecialistaID)
        {
            IEnumerable<Turno> turnospacientes = (from turnos in db.Turnos
                                                  join especialistas in db.Especialistas on turnos.Especialista.Id equals especialistas.Id
                                                  where especialistas.Id == EspecialistaID && turnos.EstadoTurno == Estado.Pendiente
                                                  select turnos).ToList();

            ICollection<TurnosViewModel> _turnosvm = turnospacientes.Select(b => new TurnosViewModel
            {
                IdTurno = b.IdTurno,
                Dia = b.Dia.Date,
                HoraInicio = b.HoraInicio,
                HoraFin = b.HoraFin,
                Precio = b.Precio,
                EstadoTurno = b.EstadoTurno,
                Especialista = b.Especialista,
                Paciente = b.Paciente
            }).ToList();

            var viewmodel = new DashboardEspecialistaVM()
            {
                TurnosViewModel = _turnosvm
            };
            return viewmodel;
        }



        [HttpGet]
        [Authorize]
        public DashboardAdministradorVM DashboardAdministrator()
        {
            IEnumerable<Turno> _turnos = (from t in db.Turnos
                                         where t.EstadoTurno == Estado.Pendiente
                                         select t).ToList();

            IEnumerable<Paciente> _pacientes = (from t in db.Pacientes
                                               where t.Estado == true
                                               select t).ToList();

            IEnumerable<Especialista> _especialistas = (from t in db.Especialistas
                                                       where t.Estado == true
                                                       select t).ToList();

            var _ordenturno = (from e in db.OrdenTurnos
                               where e.Status == "approved"
                               orderby e.FechaCreacion
                               group e by new { data = e.FechaCreacion.Month } into e
                               select new {
                                   e.Key.data
                               }).ToList();

            var _reportemensual = (from e in db.ReporteConsulta.ToList()
                                   orderby e.MesReporte.Month ascending
                                   where e.MesReporte.AddMonths(6) > DateTime.Now.AddMonths(-6) && e.MesReporte.Year == 2017
                                   group e by new { data = e.CantidadConsultas20M } into g
                                   select new
                                   {
                                       g.Key.data
                                   }).ToList();
            
            ViewBag.reportemensual = JsonConvert.SerializeObject(_reportemensual);
            ViewBag.ordenturno = JsonConvert.SerializeObject(_ordenturno);
            //ViewBag.reportemensual = _reportemensual;
            //ViewBag.turnos20M = turnos20M;
            //ViewBag.turnos30M = turnos30M;
            var viewmodel = new DashboardAdministradorVM()
            {
                turnosvm = _turnos.ToList(),
                especialistas = _especialistas.ToList(),
                pacientes = _pacientes.ToList(),
                reporteconsultaanual = null,
           };

            return viewmodel;
        }

        public JsonResult SendNotification(string username, string message)
        {
            return Json(true);
        }
    }
}