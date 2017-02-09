using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.Aplicacion
{
    public class Tarjeta
    {
        public Tarjeta()
        {
            this.IdTarjeta = Guid.NewGuid().ToString();
        }
        [Key]
        public string IdTarjeta { get; set; }
        public string TipoDeTarjeta { get; set; }
        public string NumeroTarjeta { get; set; }
        public int MesExpiracion { get; set; }
        public int AnoExpiracion { get; set; }
        public string CodigoSeguridad { get; set; }
        public string Estado { get; set; }
        public ClienteTarjeta Cliente { get; set; }
    }

    public class ClienteTarjeta
    {
        public ClienteTarjeta()
        {
            this.IdClienteTarjeta = Guid.NewGuid().ToString();
        }

        [Key]
        public string IdClienteTarjeta { get; set; }
        public string NombreCliente { get; set; }
        public int DNI { get; set; }
        public ICollection<Tarjeta> Tarjetas { get; set; }
    }

    public enum EstadoTarjeta
    {
        Habilitada,
        Rechazada
    }

    public enum TipoTarjeta
    {
        Visa = 1,
        Mastercard = 2,
        AmericanExpress = 3,
        Discover = 4,
        Amex = 5,
        Maestro = 6,
        MasterCard = 7,
        MasterCardDebit = 8,
        VisaDebit = 9
    }

    public enum Mes
    {
        Enero = 1,
        Febrero = 2,
        Marzo = 3,
        Abril = 4,
        Mayo = 5,
        Junio = 6,
        Julio = 7,
        Agosto = 8,
        Septiembre = 9,
        Octubre = 10,
        Noviembre = 11,
        Diciembre = 12
    }

    public enum AnoExpiracion : int
    {
        AnoActual = 2016,
        AnoProx1 = 2017,
        AnoProx2 = 2018,
        AnoProx3 = 2019,
        AnoProx4 = 2020,
        AnoProx5 = 2021
    }
}