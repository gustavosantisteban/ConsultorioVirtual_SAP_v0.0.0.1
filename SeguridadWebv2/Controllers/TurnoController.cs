using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.Aplicacion;
using SeguridadWebv2.Services;
using SeguridadWebv2.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    [Authorize]
    public class TurnoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Turno
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ReservarTurno(string id, string espec)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var showhorario = db.HorariosDisponibles.Where(x => x.Id == id).FirstOrDefault();
            var especialista = db.Especialistas.Where(x => x.Id == espec).FirstOrDefault();

            if (showhorario == null)
            {
                return HttpNotFound();
            }

            var tipotarjeta = from TipoTarjeta d in Enum.GetValues(typeof(TipoTarjeta))
                              select new { IdTipoTarjeta = (int)d, Name = d.ToString() };

            var mestarjeta = from Mes d in Enum.GetValues(typeof(Mes))
                              select new { IdMesTarjeta = (int)d, Name = (int)Enum.Parse(typeof(Mes), d.ToString()) };

            var anotarjeta = from AnoExpiracion d in Enum.GetValues(typeof(AnoExpiracion))
                             select new { IdAnoExp = (int)d, Name = (int)Enum.Parse(typeof(AnoExpiracion), d.ToString()) };



            ViewBag.IdTipoTarjeta = new SelectList(tipotarjeta, "IdTipoTarjeta", "Name");
            ViewBag.IdMesTarjeta = new SelectList(mestarjeta, "IdMesTarjeta", "Name");
            ViewBag.IdAnoExp = new SelectList(anotarjeta, "IdAnoExp", "Name");

            ViewBag.IdHorarioShowView = showhorario.Id;
            ViewBag.IdEspecislistaShowView = especialista.Id;

            var model = new TurnoReservaViewModel
            {
                TurnoViewModel = new TurnoViewModel
                {
                    Horario = db.HorariosDisponibles.Include("Horario").Single(x => x.Id == showhorario.Id),
                },
                TarjetaViewModel = new TarjetaViewModel()
            }; 
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ConfirmarPagoReserva(TurnoReservaViewModel model)
        {
            if (ModelState.IsValid)
            {
                string validartarjeta = TarjetaCreditoValidation.ValidateCardNumber(model.TarjetaViewModel.NumeroTarjeta);
                bool c = TarjetaCreditoValidation.IsValidCardType(TarjetaCreditoValidation.CleanCreditCardNumber(model.TarjetaViewModel.NumeroTarjeta));

                //TurnoReserva turnoreserva = new TurnoReserva();
                //turnoreserva.UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                //turnoreserva.Estado = Estado.Reservado;

                string currentUserId = User.Identity.GetUserId();
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                Paciente paciente = db.Pacientes.ToList().FirstOrDefault(x => x.Id == currentUser.Id);
                
                var horarioDisponible = db.HorariosDisponibles.Include("Horario").FirstOrDefault(x => x.Id == model.TurnoViewModel.Horario.Id);
                //var horarioDisponible = _db.HorariosDisponibles.Where(x => x.Disponible == true).OrderBy(x => x.Dia).Include(h => h.Horario).Include(e => e.Horario.Especialista).FirstOrDefault(x => x.Id == HorarioDisponible.Id); //.Find(Id);

                Relacion relacion = new Relacion();
                bool existeRelacion = false;
                foreach (Relacion relaciones in db.RelacionPacienteEspecialista.Include("Paciente").Include("Especialista"))
                {
                    if (relaciones.Especialista.EspecialidadId == horarioDisponible.Horario.EspecialistaId && relaciones.Paciente.Id == paciente.Id)
                    {
                        existeRelacion = true;
                        relacion = relaciones;
                    }
                }

                if (existeRelacion != true)
                {
                    relacion.Especialista = horarioDisponible.Horario.Especialista;
                    relacion.Paciente = paciente;

                    Turno turno = new Turno();
                    turno.Dia = horarioDisponible.Dia;
                    turno.HoraInicio = horarioDisponible.HoraInicio;
                    turno.HoraFin = horarioDisponible.HoraFin;
                    turno.RelacionPacienteEspecialista = relacion;
                    turno.EstadoTurno = Estado.Pendiente;
                    relacion.Turnos.Add(turno);
                    horarioDisponible.Disponible = EstadoHorario.Ocupado;
                    relacion.FechaRelacion = DateTime.Now;
                    db.RelacionPacienteEspecialista.Add(relacion);
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    var turno = new Turno();

                    turno.Dia = horarioDisponible.Dia;
                    turno.HoraInicio = horarioDisponible.HoraInicio;
                    turno.HoraFin = horarioDisponible.HoraFin;
                    turno.RelacionPacienteEspecialista = relacion;
                    turno.EstadoTurno = Estado.Pendiente;
                    relacion.Turnos.Add(turno);
                    horarioDisponible.Disponible = EstadoHorario.Disponible;
                    db.SaveChanges();
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            return View(model);
        }
    }
}