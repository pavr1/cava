using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cava.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        [Required]
        [Display(Name = "Fecha de Reservación")]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Número de Personas")]
        public int NumberOfPeople { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string ReserverFirstName { get; set; }
        [Required]
        [Display(Name = "Apellidos")]
        public string ReserverMiddleName { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? DOB { get; set; }
        [Required]
        [Display(Name = "Número Telefónico")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
    }
}