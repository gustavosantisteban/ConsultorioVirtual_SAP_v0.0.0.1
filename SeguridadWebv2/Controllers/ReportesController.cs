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
using System.Data.SqlClient;
using SeguridadWebv2.Models.ReportClass;
using System.Data.Entity.Core.Objects;

namespace SeguridadWebv2.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult JsonPieChartEspecialistaTurnos()
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            var rol = UserManager.GetRoles(usuario);

            var especialistasTurnosCount = (from turnos in db.Turnos
                                            join relacion in db.RelacionPacienteEspecialista on turnos.RelacionId equals relacion.IdRelacion
                                            join especialistas in db.Especialistas on relacion.Especialista.Id equals especialistas.Id
                                            where especialistas.Id == usuario && turnos.EstadoTurno == Estado.Pendiente
                                            select new
                                            {
                                               estadoturno = turnos.EstadoTurno,
                                               dia = turnos.Dia
                                            }).ToList();

            return Json(especialistasTurnosCount, JsonRequestBehavior.AllowGet);
        }


        public JsonResult JsonGetEspConsultasTurnos()
        {
            IEnumerable<EspConsultaTurno> resultado;
            using (var context = new ApplicationDbContext())
            {
                ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var usuario = User.Identity.GetUserId();

                SqlParameter idespecialista = new SqlParameter("@IdEspecialista", usuario);
                resultado = context.Database.SqlQuery<EspConsultaTurno>("NewCategory @CategoryName", idespecialista).ToList<EspConsultaTurno>();
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
    }
}