using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models.ViewModels;

namespace SeguridadWebv2.Controllers
{
    public class HorarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Horario
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var horarios = db.Horarios.Include("Especialista").Include("HorariosDisponibles").ToList();
            
            IEnumerable<HorarioViewModel> _horariosvm = horarios.Select(b => new HorarioViewModel
            {
                IDHorario = b.IDHorario,
                Especialista = b.Especialista,
                CantidadTurnos = b.CantidadTurnos,
                Dia = b.Dia,
                Duracion = b.Duracion,
                Estado = b.Estado,
                HoraInicio = b.HoraInicio,
                HorariosDisponibles = b.HorariosDisponibles.ToList(),
                Mes = b.Mes
            })
            .ToList();
            return View(_horariosvm.ToList());
        }
        
        [HttpGet]
        [Authorize(Roles = "Profesionales, Detalle_Horario, Profesional")]
        public ActionResult MisHorarios()
        {
            var EspecialistaID = User.Identity.GetUserId();

            if (EspecialistaID == null)
            {
                return HttpNotFound();
            }

            var horarios = db.Horarios.Include("Especialista").Include("HorariosDisponibles").Where(x => x.EspecialistaId == EspecialistaID).ToList();

            IEnumerable<HorarioViewModel> _horariosvm = horarios.Select(b => new HorarioViewModel
            {
                IDHorario = b.IDHorario,
                Especialista = b.Especialista,
                CantidadTurnos = b.CantidadTurnos,
                Dia = b.Dia,
                Duracion = b.Duracion,
                Estado = b.Estado,
                HoraInicio = b.HoraInicio,
                HorariosDisponibles = b.HorariosDisponibles.ToList(),
                Mes = b.Mes
            })
            .ToList();
            return View(_horariosvm.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Profesionales, Detalle_Horario, Profesional")]
        public ActionResult Detalle(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horarioModels = db.Horarios.Include(i => i.Especialista)
                                  .Include(i => i.HorariosDisponibles)
                                  .SingleOrDefault(x => x.IDHorario == id);

            if (horarioModels == null)
            {
                return HttpNotFound();
            }

            HorarioViewModel _horariosvm = new HorarioViewModel
            {
                IDHorario = horarioModels.IDHorario,
                Especialista = horarioModels.Especialista,
                CantidadTurnos = horarioModels.CantidadTurnos,
                Dia = horarioModels.Dia,
                Duracion = horarioModels.Duracion,
                Estado = horarioModels.Estado,
                HoraInicio = horarioModels.HoraInicio,
                HorariosDisponibles = horarioModels.HorariosDisponibles.OrderBy(x => x.Dia.Date).ToList(),
                Mes = horarioModels.Mes
            };
            
            return View(_horariosvm);
        }

        // GET: Horario/Create
        [HttpGet]
        [Authorize(Roles = "Admin, Profesionales, Agregar_Horario, Profesional")]
        public ActionResult Create()
        {
            var horarioModel = new HorarioViewModel();
            return View(horarioModel);
        }

        // POST: Horario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Duracion,HoraInicio,CantidadTurnos,Mes,Dia")] HorarioViewModel horarioModels)
        {
            if (ModelState.IsValid)
            {
                //db.Horarios.Add(horarioModels);
                //db.SaveChanges();
                horarioModels.EspecialistaID = User.Identity.GetUserId();
                horarioModels.Estado = EstadoHorario.Disponible;
                //Especialista usuario = db.Especialistas.Where(x => x.Id.Contains(horarioModels.EspecialistaID)).FirstOrDefault();
                List<DateTime> lstSundays = new List<DateTime>();

                int intYear = DateTime.Now.Year;
                int intDaysThisMonth = DateTime.DaysInMonth(intYear, (int)horarioModels.Mes /*intMonth*/);
                DateTime oBeginnngOfThisMonth = new DateTime(intYear, (int)horarioModels.Mes/*intMonth*/, 1);
                for (int i = 1; i < intDaysThisMonth + 1; i++)
                {
                    if (oBeginnngOfThisMonth.AddDays(i - 1).DayOfWeek == (DayOfWeek)Convert.ToInt32(horarioModels.Dia))
                    {
                        lstSundays.Add(new DateTime(intYear, (int)horarioModels.Mes/*intMonth*/, i));
                    }
                }
                
                foreach (DateTime dia in lstSundays)
                {
                    int totalTurnos = horarioModels.CantidadTurnos;
                    DateTime ultimaHora = DateTime.Now;
                    while (totalTurnos != 0)
                    {
                        HorarioDisponible horarioDisponible = new HorarioDisponible();
                        horarioDisponible.Dia = dia;
                        horarioDisponible.HorarioId = horarioModels.IDHorario;
                        if (horarioModels.CantidadTurnos == totalTurnos)
                        {
                            horarioDisponible.HoraInicio = horarioModels.HoraInicio;
                            horarioDisponible.HoraFin = horarioDisponible.HoraInicio.AddMinutes(horarioModels.Duracion);
                            totalTurnos -= 1;
                            ultimaHora = horarioDisponible.HoraFin;
                        }
                        else
                        {
                            horarioDisponible.HoraInicio = ultimaHora;
                            horarioDisponible.HoraFin = horarioDisponible.HoraInicio.AddMinutes(horarioModels.Duracion);
                            totalTurnos -= 1;
                            ultimaHora = horarioDisponible.HoraFin;
                        }
                        horarioModels.HorariosDisponibles.Add(horarioDisponible);
                    }
                }

                var horario = new Horario
                {
                    Dia = horarioModels.Dia,
                    Duracion = horarioModels.Duracion,
                    HoraInicio = horarioModels.HoraInicio,
                    Estado = horarioModels.Estado,
                    Mes = horarioModels.Mes,
                    HorariosDisponibles = horarioModels.HorariosDisponibles,
                    CantidadTurnos = horarioModels.CantidadTurnos,
                    EspecialistaId = horarioModels.EspecialistaID
                };

                db.Horarios.Add(horario);
                db.SaveChanges();
            }
            return View("Dashboard", "Home");
        }

        // GET: Horario/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin, Profesionales, Editar_Horario, Profesional")]
        public ActionResult Editar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horarioModels = db.Horarios.Find(id);
            if (horarioModels == null)
            {
                return HttpNotFound();
            }
            return View(horarioModels);
        }

        // POST: Horario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Duracion,HoraInicio,CantidadTurnos,EspecialistaID")] Horario horarioModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(horarioModels).State = EntityState.Modified;
                db.SaveChanges();
                return View("Dashboard", "Home");
            }
            return View(horarioModels);
        }

        // GET: Horario/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin, Profesionales, Eliminar_Horario, Profesional")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Horario horarioModels = db.Horarios.Find(id);
            if (horarioModels == null)
            {
                return HttpNotFound();
            }
            return View(horarioModels);
        }

        // POST: Horario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Horario horarioModels = db.Horarios.Find(id);
            db.Horarios.Remove(horarioModels);
            db.SaveChanges();
            return View("Dashboard", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        [HttpGet]
        public ActionResult ListHorarioDisponibles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorarioDisponible horarioDisponible = db.HorariosDisponibles.Find(id);
            if (horarioDisponible == null)
            {
                return HttpNotFound();
            }
            return PartialView("_ListHorarioDisponibles", horarioDisponible);
        }

    }
}