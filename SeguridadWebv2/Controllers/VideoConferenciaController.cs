using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OpenTokSDK;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    [Authorize]
    public class VideoConferenciaController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: VideoConferencia
        public ActionResult Iniciar(string id)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var rol = UserManager.GetRoles(usuario);
            var turno = db.Turnos.Where(x => x.IdTurno == id).FirstOrDefault();
            ViewBag.FechaFin = (turno.HoraFin - DateTime.Now);


            //https://github.com/aoberoi/OpenTok-DotNet-Sample/blob/master/OpenTokSample/Web.config
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            ViewBag.APIKey = appSettings["opentok_key"];
            ViewBag.ApiSecret = appSettings["api_secret"];
            //OpenTok opentok = new OpenTok();

            //string sessionId = opentok.CreateSession(Request.ServerVariables["REMOTE_ADDR"]);

            //Dictionary<string, object> tokenOptions = new Dictionary<string, object>();
            //tokenOptions.Add(TokenPropertyConstants.ROLE, RoleConstants.MODERATOR);
            //ViewBag.Token = opentok.GenerateToken(sessionId, tokenOptions);
            //return View();

            return View();
        }

        public ActionResult SubirArchivosConsulta()
        {
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

            return Json(new { Message = string.Empty });
        }

        public ActionResult UploadFilesSharedPaciente(FilesSharedViewModel vm)
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            if (usuario == null)
            {
                return HttpNotFound();
            }
            var rol = UserManager.GetRoles(usuario);
            if (rol.Contains("Paciente"))
            {
                var relacion = db.RelacionPacienteEspecialista.Where(x => x.Paciente.Id == usuario).FirstOrDefault();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<FileSharedRelacion, FilesSharedViewModel>()
                        .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now))
                        .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                        .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => relacion.Paciente.Id))
                        .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true))
                        .ForMember(dest => dest.ToUser, opt => opt.MapFrom(src => relacion.Especialista.Id))
                        .ForMember(dest => dest.IdRelacion, opt => opt.MapFrom(src => src.IdRelacion))
                        .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.IdFile));
                });
            }
            if (rol.Contains("Profesionales"))
            {
                var relacion = db.RelacionPacienteEspecialista.Where(x => x.Paciente.Id == usuario).FirstOrDefault();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<FileSharedRelacion, FilesSharedViewModel>()
                        .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => DateTime.Now))
                        .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => true))
                        .ForMember(dest => dest.FromUser, opt => opt.MapFrom(src => relacion.Especialista.Id))
                        .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true))
                        .ForMember(dest => dest.ToUser, opt => opt.MapFrom(src => relacion.Paciente.Id))
                        .ForMember(dest => dest.IdRelacion, opt => opt.MapFrom(src => src.IdRelacion))
                        .ForMember(dest => dest.IdFile, opt => opt.MapFrom(src => src.IdFile));
                });
            }

            return View();
        } 


        public ActionResult MisArchivosCompartidos()
        {
            ApplicationUserManager UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var usuario = User.Identity.GetUserId();
            var paciente = db.Pacientes.Find(usuario);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            if (paciente == null)
            {
                return HttpNotFound();
            }
            try
            {
               //resultado = db.FileSharedRelacion.Where(x => x.FromUser == paciente.Id && x.Estado == true).ToList();
            } catch (Exception ex)
            {
                @ViewBag.Error = ex.Message;
            }
            //return PartialView("_FileSharedPaciente", resultado);
            return View();
        }

    }
}