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

            //IQueryable<Especialista> esp = db.Especialistas.Where(x => x.Especialidad.EspecialidadId == id.EspecialidadId);
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
                                                  join relacion in db.RelacionPacienteEspecialista on turnos.RelacionId equals relacion.IdRelacion
                                                  join paciente in db.Pacientes on relacion.Paciente.Id equals paciente.Id
                                                  where relacion.Paciente.Id == PacienteID && turnos.EstadoTurno == Estado.Pendiente
                                                  select turnos).ToList();


            ICollection<TurnosViewModel> _turnosvm = turnospacientes.Select(b => new TurnosViewModel
            {
                IdTurno = b.IdTurno,
                Dia = b.Dia.Date,
                HoraInicio = b.HoraInicio,
                HoraFin = b.HoraFin,
                EstadoTurno = b.EstadoTurno,
                Especialista = b.RelacionPacienteEspecialista.Especialista
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
                                                  join relacion in db.RelacionPacienteEspecialista on turnos.RelacionId equals relacion.IdRelacion
                                                  join especialistas in db.Especialistas on relacion.Especialista.Id equals especialistas.Id
                                                  where especialistas.Id == EspecialistaID && turnos.EstadoTurno == Estado.Pendiente
                                                  select turnos).ToList();

            ICollection<TurnosViewModel> _turnosvm = turnospacientes.Select(b => new TurnosViewModel
            {
                IdTurno = b.IdTurno,
                Dia = b.Dia.Date,
                HoraInicio = b.HoraInicio,
                HoraFin = b.HoraFin,
                EstadoTurno = b.EstadoTurno,
                Especialista = b.RelacionPacienteEspecialista.Especialista,
                Paciente = b.RelacionPacienteEspecialista.Paciente
            }).ToList();

            var viewmodel = new DashboardEspecialistaVM()
            {
                TurnosViewModel = _turnosvm
            };
            return viewmodel;
        }

        public JsonResult SendNotification(string username, string message)
        {
            return Json(true);
        }
    }
}