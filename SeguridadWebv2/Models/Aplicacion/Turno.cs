using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    [Table("Turnos")]
    public class Turno : IObserver<HorarioDisponible>
    {
        private IDisposable unsubscriber;
        private int intState;
        public Turno()
        {
            this.IdTurno = Guid.NewGuid().ToString();
            this.EstadoTurno = Estado.Reservado;
        }

        [Key]
        public string IdTurno { get; set; }
        public DateTime Dia { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public Estado EstadoTurno { get; set; }
        public string RelacionId { get; set; }

        [ForeignKey("RelacionId")]
        public virtual Relacion RelacionPacienteEspecialista { get; set; }

        public Turno(int state)
        {
            this.intState = state;
        }

        public int State
        {
            get { return this.intState; }
        }

        public virtual void Suscribe(IObservable<HorarioDisponible> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public void OnNext(HorarioDisponible value)
        {
            string id = value.Disponible.ToString();
        }

        public void OnError(Exception error)
        {
            unsubscriber.Dispose();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }

    public enum Estado
    {
        Reservado = 1,
        Cancelado = 2,
        Pendiente = 3,
        Confirmado = 4,
        Finalizado = 5,
        Rechazado = 6,
        Anulado = 7
    }
}