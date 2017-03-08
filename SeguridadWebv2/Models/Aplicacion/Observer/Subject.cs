//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace SeguridadWebv2.Models.Aplicacion.Observer
//{
//    public abstract class Subject
//    {
//        private EstadoHorario _stado;
//        private List<IObserverTurno> _investors = new List<IObserverTurno>();
//        // Constructor
//        public Subject()
//        {

//        }
//        public Subject(EstadoHorario stado)
//        {
//            this._stado = stado;
//        }
//        public void Attach(IObserverTurno investor)
//        {
//            _investors.Add(investor);
//        }
//        public void Detach(IObserverTurno investor)
//        {
//            _investors.Remove(investor);
//        }
//        public void Notify()
//        {
//            foreach (IObserverTurno investor in _investors)
//            {
//                investor.Update(this);
//            }
//            Console.WriteLine("");
//        }
//        // Gets or sets the price
//        public EstadoHorario EstadoHorario
//        {
//            get { return _stado; }
//            set
//            {
//                if (_stado != value)
//                {
//                    _stado = value;
//                    Notify();
//                }
//            }
//        }
//    }
//}