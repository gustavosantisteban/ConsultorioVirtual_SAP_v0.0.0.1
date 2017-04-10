using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using OpenTokSDK;
using OpenTokSDK.Exception;
using SeguridadWebv2.Helpers;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    [Authorize]
    public class VideoConferenciaController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private OpenTok opentok = new OpenTok(Convert.ToInt32(ConfigurationManager.AppSettings["opentok_key"]),
                                        ConfigurationManager.AppSettings["opentok_secret"]);
        // GET: VideoConferencia
        [SessionExpire]
        public async Task<ActionResult> Iniciar(string id)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var rol = UserManager.GetRoles(usuario);
            var turno = db.Turnos.Include("Paciente").Include("Especialista").Where(x => x.IdTurno == id).FirstOrDefault();
            var historiaclinica = db.HistoriaClinica.Find(turno.Paciente.Id);
            var consultaviewmodel = new ConsultaViewModel
            {
                Calificacion = null,
                HistoriaClinica = historiaclinica,
                TurnoVM = new TurnosViewModel()
                {
                    Dia = turno.Dia,
                    Especialista = turno.Especialista,
                    Paciente = turno.Paciente,
                    HoraInicio = turno.HoraInicio,
                    HoraFin = turno.HoraFin,
                    RelacionId = turno.RelacionId
                }
            };
            try
            {
                string sessionId = GetSessionId(HttpContext.ApplicationInstance.Application);
                ViewBag.apikey = opentok.ApiKey;
                ViewBag.sessionId = sessionId;
                ViewBag.token = opentok.GenerateToken(sessionId);
            }
            catch (OpenTokException)
            {
                ViewBag.errorMessage = "No se puedo generar el token";
            }

            ////https://github.com/aoberoi/OpenTok-DotNet-Sample/blob/master/OpenTokSample/Web.config
            //NameValueCollection appSettings = ConfigurationManager.AppSettings;
            //ViewBag.APIKey = appSettings["opentok_key"];
            //ViewBag.ApiSecret = appSettings["api_secret"];
            //OpenTok opentok = new OpenTok();

            //string sessionId = opentok.CreateSession(Request.ServerVariables["REMOTE_ADDR"]);

            //Dictionary<string, object> tokenOptions = new Dictionary<string, object>();
            //tokenOptions.Add(TokenPropertyConstants.ROLE, RoleConstants.MODERATOR);
            //ViewBag.Token = opentok.GenerateToken(sessionId, tokenOptions);
            //return View();

            return View(consultaviewmodel);
        }

        private string GetSessionId(HttpApplicationState Application)
        {

            if (Application["sessionId"] == null)
            {
                Application.Lock();
                Application["sessionId"] = opentok.CreateSession().Id;
                Application.UnLock();
            }
            return (string)Application["sessionId"];

        }

        public ActionResult SubirArchivosConsulta()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            try
            {
                var filefolder = string.Empty;
                var savefile = "";
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    string folder = User.Identity.Name;
                    var path = Server.MapPath("~/Content/img/" + folder);
                    var directory = new DirectoryInfo(path);

                    if (directory.Exists == false)
                    {
                        directory.Create();
                        savefile = Path.Combine(Server.MapPath(path), System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(savefile);
                        Models.Aplicacion.File archivo = new Models.Aplicacion.File();
                        archivo.Estado = true;
                        archivo.Fecha = DateTime.Now;
                        archivo.FullPath = savefile.ToString();
                        archivo.IdUsuario = User.Identity.GetUserId();
                        db.Files.Add(archivo);
                    }
                    else
                    {
                        savefile = Path.Combine(path, System.IO.Path.GetFileName(file.FileName));
                        file.SaveAs(savefile);
                        Models.Aplicacion.File archivo = new Models.Aplicacion.File();
                        archivo.Estado = true;
                        archivo.Fecha = DateTime.Now;
                        archivo.FullPath = savefile.ToString();
                        archivo.IdUsuario = User.Identity.GetUserId();
                        db.Files.Add(archivo);
                    }
                    db.SaveChanges();
                    //You can Save the file content here
                }
            } catch (Exception ex)
            {
                @ViewBag.Error = ex.Message;
            }

            if (isSavedSuccessfully)
            {
                string folder = User.Identity.Name;
                var path = Server.MapPath("~/Content/img/" + folder);
                string targetPath = Path.Combine(path, fName);

                var result = db.Files.Where(x => x.FullPath == targetPath).FirstOrDefault();
                var resultado = Json(result, JsonRequestBehavior.AllowGet);
                return Json(new { archivo = resultado.Data });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
            //var attachmentlist = db.Files.ToList();

            //return Json(new { Message = fName }, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult SaveUploadedFile()
        //{
        //    bool isSavedSuccessfully = true;
        //    string fName = "";
        //    foreach (string fileName in Request.Files)
        //    {
        //        HttpPostedFileBase file = Request.Files[fileName];
        //        //Save file content goes here
        //        fName = file.FileName;
        //        if (file != null && file.ContentLength > 0)
        //        {

        //            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

        //            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

        //            var fileName1 = Path.GetFileName(file.FileName);


        //            bool isExists = System.IO.Directory.Exists(pathString);

        //            if (!isExists)
        //                System.IO.Directory.CreateDirectory(pathString);

        //            var path = string.Format("{0}\\{1}", pathString, file.FileName);
        //            file.SaveAs(path);

        //        }

        //    }

        //    if (isSavedSuccessfully)
        //    {
        //        return Json(new { Message = fName });
        //    }
        //    else
        //    {
        //        return Json(new { Message = "Error in saving file" });
        //    }
        //}

        [HttpPost]
        public ActionResult UploadFilesSharedPaciente(FilesSharedViewModel vm)
        {
            vm.IdFile = "bf53298a-dd2a-4b6c-83be-56f6a4e7be76";
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var rol = UserManager.GetRoles(usuario);
            if (rol.Contains("Paciente"))
            {
                var relacion = db.Turnos.Where(x => x.Paciente.Id == usuario).FirstOrDefault();
                //Mapper.Initialize(cfg =>
                //{
                //    cfg.CreateMap<FileSharedRelacion, FilesSharedViewModel>()
                //        .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now))
                //        .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                //        .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => relacion.Paciente.Id))
                //        .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true))
                //        .ForMember(dest => dest.ToUser, opt => opt.MapFrom(src => relacion.Especialista.Id))
                //        .ForMember(dest => dest.IdRelacion, opt => opt.MapFrom(src => src.IdRelacion))
                //        .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.IdFile));
                //});
                //Mapper.Map<FilesSharedViewModel, FileSharedRelacion>(vm);
                var model = new FileSharedRelacion()
                {
                    Estado = vm.Estado,
                    FromUser = vm.FromUser,
                    ToUser = vm.ToUser,
                    Fecha = DateTime.Now,
                    IdFile = vm.IdFile,
                    Success = vm.Success,
                    IdRelacion = vm.IdRelacion
                };
                db.FileSharedRelacion.Add(model);
                db.SaveChanges();
            }
            if (rol.Contains("Profesionales"))
            {
                var relacion = db.Turnos.Where(x => x.Paciente.Id == usuario).FirstOrDefault();
                //Mapper.Initialize(cfg =>
                //{
                //    cfg.CreateMap<FileSharedRelacion, FilesSharedViewModel>()
                //        .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now))
                //        .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                //        .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => relacion.Especialista.Id))
                //        .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true))
                //        .ForMember(dest => dest.ToUser, opt => opt.MapFrom(src => relacion.Paciente.Id))
                //        .ForMember(dest => dest.IdRelacion, opt => opt.MapFrom(src => src.IdRelacion))
                //        .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.IdFile));
                //});
                //db.FileSharedRelacion.Add(Mapper.Map<FilesSharedViewModel, FileSharedRelacion>(vm));
                var model = new FileSharedRelacion()
                {
                    Estado = vm.Estado,
                    FromUser = vm.FromUser,
                    ToUser = vm.ToUser,
                    Fecha = DateTime.Now,
                    IdFile = vm.IdFile,
                    IdFileSharedRelacion = vm.IdFileSharedRelacion,
                    Success = vm.Success,
                    IdRelacion = vm.IdRelacion
                };
                db.FileSharedRelacion.Add(model);
                db.SaveChanges();
            }
            return View();
        } 

        [HttpGet]
        public ActionResult MisArchivosCompartidos(string id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;

            List<Models.Aplicacion.File> archivos = new List<Models.Aplicacion.File>();
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            var resultado = db.Turnos.Where(x => x.IdTurno == id).FirstOrDefault();
            //var paciente = db.Pacientes.Where(x => x.Id == resultado.Paciente.Id).FirstOrDefault();
            //var profesional = db.Especialistas.Where(x => x.Id == resultado.Especialista.Id).FirstOrDefault();
            var rol = UserManager.GetRoles(usuario);
            if (rol.Contains("Paciente"))
            {
                var result = db.FileSharedRelacion.Include("File").Where(x => x.IdRelacion == resultado.IdTurno).FirstOrDefault();
                if (result != null)
                {
                    Models.Aplicacion.File ok = result.File;
                    ok.FullPath = ok.FullPath.Replace(@"\\", @"\");
                    return Json(new { Url = Url.Action("ShowData", ok) });

                    //return PartialView("~/Views/VideoConferencia/_UploadFiles.cshtml", ok);
                } else
                {
                    return null;
                }
            }
            if (rol.Contains("Paciente"))
            {
                var result = db.FileSharedRelacion.Include("File").Where(x => x.IdRelacion == resultado.IdTurno).FirstOrDefault();
                Models.Aplicacion.File ok = result.File;
                ok.FullPath = ok.FullPath.Replace(@"\\", @"\");
                return PartialView("~/Views/VideoConferencia/_UploadFiles.cshtml", result.File);
            }
            return null;
        }

        public ActionResult ShowData()
        {
            return PartialView("~/Views/VideoConferencia/_UploadFiles.cshtml");
        }

        [HttpGet]
        public ActionResult PartialCreateAmenite(string amenite)
        {
            switch (amenite)
            {
                case "0":
                    //AnamnesisCardiovascular cardio = new AnamnesisCardiovascular()
                    //{
                    //    Fecha = DateTime.Now,
                    //    Encabezado = "",
                    //    Texto = ""
                    //};
                    return PartialView("~/Views/VideoConferencia/_Anamnesis.cshtml");
                case "1":
                    //AnamnesisDisgestiva digestiva = new AnamnesisDisgestiva()
                    //{
                    //    Fecha = DateTime.Now,
                    //    Encabezado = "",
                    //    Texto = ""
                    //};
                    return PartialView("~/Views/VideoConferencia/_Anamnesis.cshtml");
                case "2":
                    AnamnesisRespiratoria anamnesis = new AnamnesisRespiratoria()
                    {
                        Fecha = DateTime.Now,
                        Encabezado = "",
                        Texto = ""
                    };
                    return PartialView("~/Views/VideoConferencia/_Anamnesis.cshtml");
            };
            return PartialView("~/Views/VideoConferencia/_Anamnesis.cshtml");
        }
        
    }
}