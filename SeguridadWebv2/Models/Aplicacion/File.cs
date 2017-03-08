using SeguridadWebv2.Models.Aplicacion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class File
    {
        public File()
        {
            this.IdFile = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdFile { get; set; }
        public string FullPath { get; set; }
        public DateTime Fecha { get; set; }
        public string IdUsuario { get; set; }
        public bool Estado { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual ApplicationUser Usuario { get; set; }
    }
}




//public class Turno : IObserver<HorarioDisponible>
//{
//    private IDisposable unsubscriber;
//    private int intState;

//    public Provider(int state)
//    {
//        this.intState = state;
//    }

//    public int State
//    {
//        get { return this.intState; }
//    }

//    public virtual void Suscribe(IObservable<ContractService> provider)
//    {
//        if (provider != null)
//            unsubscriber = provider.Subscribe(this);
//    }

//    public virtual void OnCompleted()
//    {
//        this.Unsubscribe();
//    }

//    public virtual void OnError(Exception error)
//    {

//    }

//    public void OnNext(ContractService value)
//    {
//        string id = value.State.ToString();
//    }

//    public virtual void Unsubscribe()
//    {
//        unsubscriber.Dispose();
//    }
//}