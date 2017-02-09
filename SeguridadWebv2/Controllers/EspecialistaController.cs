using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class EspecialistaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        public EspecialistaController()
        {
        }

        public EspecialistaController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Especialista
        public ActionResult Index()
        {
            return View();
        }
        
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Registrarse()
        {
            RegistrarseEspViewModel registrarse = new RegistrarseEspViewModel();
            ViewBag.Especialidades = db.Especialidades.ToList(); 
            return View(registrarse);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Registrarse(RegistrarseEspViewModel model)
        {
            model.Estado = true;
            if (ModelState.IsValid)
            {
                var especialidad = db.Especialidades.Find(model.IdEspecialidad);
                if (especialidad == null)
                {
                    return HttpNotFound();
                }
                string pathimage = "~/Content/img/doctor.png"; 
                Especialista user = new Especialista { UserName = model.Email, Email = model.Email, Nombre = model.Nombre, Apellido = model.Apellido, Estado = model.Estado, PhoneNumber = model.Telefono, NumeroMatricula = model.Matricula, EspecialidadId = model.IdEspecialidad, ImagenMedico = pathimage };
                var result = await UserManager.CreateAsync(user, model.Password);
                GrupoManager groupManager = new GrupoManager();
                var group = db.ApplicationGroups.Where(x => x.Name == "Especialistas").FirstOrDefault();
                groupManager.SetUserGroups(user.Id, group.Id);
                if (result.Succeeded)
                {
                    //await this.groupManager.SetUserGroups(user.Id, "Profesionales");
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmarEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirmar su cuenta", "Por favor para confirmar su cuenta haga click en el siguiente enlace: <a href=\"" + callbackUrl + "\">link</a>");

                    //ViewBag.Link = callbackUrl;
                    return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Turnos(string id, DateTime? dato)
        {
            if (dato == null)
            {
                DateTime date = Convert.ToDateTime(DateTime.Now);
            }
            Especialista espec = db.Especialistas.FirstOrDefault(x => x.Id == id);
            
            IEnumerable<HorarioDisponible> horariodisponible = (from horariosdisp in db.HorariosDisponibles
                                                    join horarios in db.Horarios on horariosdisp.HorarioId equals horarios.IDHorario
                                                    join especi in db.Especialistas on horarios.EspecialistaId equals especi.Id
                                                    where horarios.EspecialistaId == id && horariosdisp.Disponible == EstadoHorario.Disponible
                                                    select horariosdisp).ToList();

            var model = new GeneralViewModels
            {
                Especialista = espec,
                Horarios = horariodisponible.ToList()
            };
            return View(model);
        }


        //[HttpGet]
        //public JsonResult SearchtoDateTurno(string id, DateTime dato)
        //{
        //    Especialista espec = db.Especialistas.FirstOrDefault(x => x.EspecialistaId == id);

        //    IQueryable<Horario> horario = db.Horarios.Where(x => x.Especialista.EspecialistaId == id &&
        //                                    DbFunctions.TruncateTime(x.FechaInicio) == DbFunctions.TruncateTime(dato))
        //                                    .OrderBy(x => x.FechaInicio)
        //                                    .Include(x => x.Especialista);
        //    var data = new GeneralViewModels
        //    {
        //        Especialista = espec,
        //        Horarios = horario.ToList()
        //    };

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}