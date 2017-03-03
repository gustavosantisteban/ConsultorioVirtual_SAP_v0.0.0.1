using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeguridadWebv2.Models.Aplicacion.Observer
{
    public interface IObserverTurno
    {
        void Update(Subject turno);
    }
}
