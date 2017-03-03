using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion.Observer
{
    public abstract class Subject
    {
        private double _state;
        private List<IObserverTurno> _investors = new List<IObserverTurno>();

        // Constructor
        public Subject(int state)
        {
            this._state = state;
        }

        public void Attach(IObserverTurno investor)
        {
            _investors.Add(investor);
        }

        public void Detach(IObserverTurno investor)
        {
            _investors.Remove(investor);
        }

        public void Notify()
        {
            foreach (IObserverTurno investor in _investors)
            {
                investor.Update(this);
            }

            Console.WriteLine("");
        }

        // Gets or sets the price
        public double State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    Notify();
                }
            }
        }
    }
}