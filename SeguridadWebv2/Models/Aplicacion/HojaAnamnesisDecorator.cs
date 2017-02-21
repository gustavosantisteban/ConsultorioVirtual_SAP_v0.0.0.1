using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    //Este viene a ser la clase "Decorator" por cada FichaMedica
    public abstract class HojaAnamnesisDecorator : Anamnesis
    {
        protected Anamnesis component;
        
        public void SetComponent(Anamnesis component)
        {
            this.component = component;
        }
    }
}