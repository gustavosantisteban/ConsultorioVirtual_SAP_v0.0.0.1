using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    [Authorize]
    public class PreguntasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Preguntas
        public ActionResult Index(string mensaje)
        {
            var preguntasviewmodel = new PreguntaGeneralViewModel()
            {
                Descripcion = mensaje
            };
            return View(preguntasviewmodel);
        }

        [HttpPost]
        public ActionResult SendQuestion(PreguntaGeneralViewModel preguntaviewModel)
        {
            if(ModelState.IsValid)
            {
                Pregunta pregunta = new Pregunta()
                {
                    Descripcion = preguntaviewModel.Descripcion,
                    Email = preguntaviewModel.Email,
                    Telefono = preguntaviewModel.Telefono,
                    EstadoPregunta = Estado.Pendiente,
                    FechaPregunta = DateTime.Now
                };
                db.Preguntas.Add(pregunta);
                db.SaveChanges();
            };
            return View("Index");
        }
    }
}