using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SeguridadWebv2.Models.Aplicacion
{
    public class GeneralViewModels
    {
        public Especialista Especialista { get; set; }
        public IEnumerable<HorarioDisponible> Horarios { get; set; }
    }

    public class EspecialistaVM
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "Superaste el Maximo de Caracteres establecidos"), MinLength(3)]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El Apellido es obligatorio")]
        [MaxLength(100, ErrorMessage = "Superaste el Maximo de Caracteres establecidos"), MinLength(3)]
        public string Apellido { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "La direccion de email es requerida")]
        [EmailAddress(ErrorMessage = "Direccion de Email incorrecta")]
        public string Email { get; set; }

        [Display(Name = "Prefijo")]
        public Prefijo Prefijo { get; set; }
        [Display(Name = "Imagen")]
        public string ImagenMedico { get; set; }

        [Required(ErrorMessage = "El campo Telefono es obligatorio")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La Especialidad es obligatoria")]
        [Display(Name = "Especialidad")]
        public Especialidad Especialidad { get; set; }

        [Required(ErrorMessage = "El numero de matricula es obligatorio")]
        [Display(Name = "Numero de Matrícula")]
        [MaxLength(10, ErrorMessage = "Superaste el Maximo de Caracteres establecidos"), MinLength(3)]
        public string NumeroMatricula { get; set; }


        public ICollection<Turno> Turnos { get; set; }
        public ICollection<Relacion> Relaciones { get; set; }
        public ICollection<Horario> Horarios { get; set; }
    }

    public class PreguntasEspecialistaViewModel
    {
        public PreguntasEspecialistaViewModel()
        {
            this.Especialistas = new List<Especialista>();
            this.PreguntasVm = new PreguntaViewModel();
        }

        public PreguntaViewModel PreguntasVm;
        public ICollection<Especialista> Especialistas;
    }
    
    public class CreditCardType
    {
        public const string Unknown = null;
        public const string MasterCard = "MC";
        public const string Discovery = "DSC";
        public const string AmericanExpress = "AMEX";
        public const string Visa = "VISA";
        public const string JCB = "JCB";
        public const string BillMeLater = "BML";
        public const string PayPal = "PAYPAL";
        public const string Google = "GOOGLE";
        public const string WIRETRANSFER = "WIRED";
        public const string COD = "COD";
    }

    public class TarjetaViewModel
    {
        [Required]
        [Display(Name = "Tipo de Tarjeta")]
        public int IdTipoTarjeta { get; set; }
        [Required]
        [MaxLength(16, ErrorMessage = "El numero de tarjeta debe tener 15 Digitos"), MinLength(15)]
        [Display(Name = "Numero de Tarjeta")]
        public string NumeroTarjeta { get; set; }
        [Required]
        [Display(Name = "Mes Expiracion")]
        public int IdMesTarjeta { get; set; }
        [Required]
        [Display(Name = "Año Expiracion")]
        public int IdAnoExp { get; set; }
        [Required]
        [Display(Name = "Codigo Seguridad CVV")]
        [MaxLength(3, ErrorMessage = "Codigo de Seguridad debe tener 3 Digitos"), MinLength(3)]
        [DataType(DataType.Password)]
        public string CodigoSeguridad { get; set; }
        [Required]
        public string NombreClienteTarjeta { get; set; }
        [Required]
        public int DNIClienteTarjeta { get; set; }
    }

    public class TurnoViewModel
    {
        public HorarioDisponible Horario { get; set; }
    }

    public class TurnoReservaViewModel
    {
        public TarjetaViewModel TarjetaViewModel { get; set; }
        public TurnoViewModel TurnoViewModel { get; set; }
    }

    public class PreguntaViewModel
    {
        [Required(ErrorMessage = "El Campo Pregunta es obligatorio")]
        [MaxLength(500, ErrorMessage = "Superaste el Maximo de Caracteres establecidos"), MinLength(15)]
        [Display(Name = "Pregunta")]
        public string Descripcion { get; set; }
    }
    public class PreguntaGeneralViewModel
    {
        [Required(ErrorMessage = "El Campo Pregunta es obligatorio")]
        [MaxLength(500, ErrorMessage = "Superaste el Maximo de Caracteres establecidos"), MinLength(15)]
        [Display(Name = "Pregunta")]
        public string Descripcion { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "La direccion de email es requerida")]
        [EmailAddress(ErrorMessage = "Direccion de Email incorrecta")]
        public string Email { get; set; }

        public DateTime FechaPregunta { get; set; }
        public Estado EstadoPregunta { get; set; }
    }
}