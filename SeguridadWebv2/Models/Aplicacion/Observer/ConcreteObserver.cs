//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace SeguridadWebv2.Models.Aplicacion.Observer
//{
//    public class ConcreteObserver : IObservable<HorarioDisponible>
//    {
//        public List<IObserver<HorarioDisponible>> contractservicesobservers;
//        public List<HorarioDisponible> contractservices;

//        public ConcreteObserver()
//        {
//            contractservicesobservers = new List<IObserver<HorarioDisponible>>();
//            contractservices = new List<HorarioDisponible>();
//        }

//        public IDisposable Subscribe(IObserver<HorarioDisponible> observer)
//        {
//            if (!contractservicesobservers.Contains(observer))
//            {
//                contractservicesobservers.Add(observer);
//                foreach (var item in contractservices)
//                    observer.OnNext(item);
//            }
//            return new Unsubscriber<HorarioDisponible>(contractservicesobservers, observer);
//        }

//        internal class Unsubscriber<HorarioDisponible> : IDisposable
//        {
//            private List<IObserver<HorarioDisponible>> _observers;
//            private IObserver<HorarioDisponible> _observer;

//            internal Unsubscriber(List<IObserver<HorarioDisponible>> observers, IObserver<HorarioDisponible> observer)
//            {
//                this._observers = observers;
//                this._observer = observer;
//            }

//            public void Dispose()
//            {
//                if (_observers.Contains(_observer))
//                    _observers.Remove(_observer);
//            }
//        }
//    }
//}