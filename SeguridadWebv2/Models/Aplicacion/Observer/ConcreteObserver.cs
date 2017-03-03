using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion.Observer
{
    public class ConcreteObserver : IObservable<HorarioDisponible>
    {
        public List<IObserver<HorarioDisponible>> horarioobservers;
        public List<HorarioDisponible> contractservices;

        public ConcreteObserver()
        {
            horarioobservers = new List<IObserver<HorarioDisponible>>();
            contractservices = new List<HorarioDisponible>();
        }

        public IDisposable Subscribe(IObserver<HorarioDisponible> observer)
        {
            if (!horarioobservers.Contains(observer))
            {
                horarioobservers.Add(observer);
                foreach (var item in contractservices)
                    observer.OnNext(item);
            }
            return new Unsubscriber<HorarioDisponible>(horarioobservers, observer);
        }

        internal class Unsubscriber<ContractService> : IDisposable
        {
            private List<IObserver<ContractService>> _observers;
            private IObserver<ContractService> _observer;

            internal Unsubscriber(List<IObserver<ContractService>> observers, IObserver<ContractService> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}