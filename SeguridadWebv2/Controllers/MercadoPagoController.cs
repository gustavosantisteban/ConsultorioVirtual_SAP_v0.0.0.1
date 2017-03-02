using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using mercadopago;
using System.Web.Mvc;
using SeguridadWebv2.Helpers;
using SeguridadWebv2.Models;
using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models.Aplicacion;
using System.Data.Entity;

namespace SeguridadWebv2.Controllers
{
    public class MercadoPagoController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: MercadoPago
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public void MercadoPago()
        //{
        //    MP mp = new MP(ConfigurationManager.AppSettings["MPClientID"], ConfigurationManager.AppSettings["MPSecret"]);

        //    //PreferencesMP pf = new PreferencesMP {
        //    //     items = new Items() {
        //    //            currency_id = "ARS",
        //    //            unit_price = 10.5M,
        //    //            quantity = 1,
        //    //            title = "Testing"
        //    //    }
        //    //};
        //    //string item = JsonConvert.SerializeObject(pf);
        //    string item = "{\"items\":[{\"title\":\"Testing\",\"quantity\":1,\"currency_id\":\"ARS\",\"unit_price\":10.5}]}";
        //    Hashtable preference = mp.createPreference(item);

        //    var resultado = (Hashtable)((ArrayList)((Hashtable)preference["response"])["items"])[0];
        //}


        [HttpPost]
        public ActionResult DoCheckout(string data)
        {
            var turno = db.HorariosDisponibles.Where(x => x.Id == data).FirstOrDefault();

            var pf = new PreferencesMP()
            {
                items = new List<Items>()
                {
                    new Items() {
                        currency_id = "ARS",
                        unit_price = turno.Precio,
                        quantity = 1,
                        title = "Turno Consultorio Virtual " + turno.Dia.ToString()
                    },
                },
            };
            MP mp = new MP(ConfigurationManager.AppSettings["MPClientID"], ConfigurationManager.AppSettings["MPSecret"]);
            mp.sandboxMode(bool.Parse(ConfigurationManager.AppSettings["MPSandbox"]));
            var datos = new
            {
                items = pf.items.Select(i => new { title = i.title, quantity = i.quantity, currency_id = i.currency_id, unit_price = i.unit_price }).ToArray(),
                back_urls = new
                {
                    success = "http://" + Request.Url.DnsSafeHost + Url.RouteUrl("CheckoutStatus"),
                    failure = "http://" + Request.Url.DnsSafeHost + Url.RouteUrl("CheckoutStatus"),
                    pending = "http://" + Request.Url.DnsSafeHost + Url.RouteUrl("CheckoutStatus")
                }
            };
            Hashtable preference = mp.createPreference(JsonConvert.SerializeObject(datos));


            string mprefid = (string)((Hashtable)preference["response"])["id"];

            var usuario = User.Identity.GetUserId();

            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            OrdenTurno orden = new OrdenTurno()
            {
                ordenitems = new List<OrdenItem>()
                {
                    new OrdenItem() {
                        currency_id = "ARS",
                        unit_price = turno.Precio,
                        quantity = 1,
                        title = "Turno Consultorio Virtual " + turno.Dia.ToString(),
                        EsValido = false,
                        IdHorarioDisponible = turno.Id
                    },
                },
                FechaCreacion = DateTime.Now,
                MPCollectionID = "",
                Status = "",
                SessionId = usuario,
                MPRefID = mprefid
            };
            db.OrdenTurnos.Add(orden);
            db.SaveChanges();
            //string MPRefID = (string)((Hashtable)preference["response"])["id"];

            return Json(new { url = (string)((Hashtable)preference["response"])[ConfigurationManager.AppSettings["MPUrl"]] });
        }

        [HttpGet]
        public ActionResult CheckoutStatus(string collection_id, string collection_status, string preference_id, string external_reference, string payment_type, string merchant_order_id)
        {
            string mpRefID = Request["preference_id"];
            string status = Request["collection_status"];
            string collectionID = Request["collection_id"];

            var orden = db.OrdenTurnos.Where(x => x.MPRefID == mpRefID).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(mpRefID) || string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(collectionID))
            {
                return Redirect("/");
            }
            else
            {
                orden.Status = status;
                orden.MPCollectionID = collectionID;
                orden.MPRefID = mpRefID;
                //db.OrdenTurnos.Attach(orden);
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();

                var ordenrelation = orden.ordenitems.Where(x => x.IdOrdenTurno == orden.IdOrdenTurno).FirstOrDefault();
                RegistrarRelacion(ordenrelation.IdHorarioDisponible);
                return View("../Turno/Status", orden);
            };
        }
        
        [HttpGet]
        public ActionResult Notification()
        {
            string topic = Request["topic"];
            string id = Request["id"];

            MP mp = new MP(ConfigurationManager.AppSettings["MPClientID"], ConfigurationManager.AppSettings["MPSecret"]);
            mp.sandboxMode(bool.Parse(ConfigurationManager.AppSettings["MPSandbox"]));

            Hashtable paymentInfo = mp.getPaymentInfo(id);

            //NotifyUserOrderStatus();
            //NotifyBuyerOrderStatus();

            //NotifyUserOrderStatus();

            string Status = ((Hashtable)((Hashtable)paymentInfo["response"])["collection"])["status"].ToString();


            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
        }


        public void RegistrarRelacion(string idhorario)
        {

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            Paciente paciente = db.Pacientes.ToList().FirstOrDefault(x => x.Id == currentUser.Id);

            var horarioDisponible = db.HorariosDisponibles.Include("Horario").FirstOrDefault(x => x.Id == idhorario);
           
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
            }
        }
    }
}