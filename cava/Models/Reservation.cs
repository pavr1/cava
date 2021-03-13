using cava.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cava.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        [Required]
        [Display(Name = "Fecha de Reservación")]
        public DateTime ReservationDate { get; set; }
        [Required]
        [Display(Name = "Número de Personas")]
        public int NumberOfPeople { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string ReserverFirstName { get; set; }
        [Required]
        [Display(Name = "Apellidos")]
        public string ReserverLastName { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? DOB { get; set; }
        [Required]
        [Display(Name = "Número Telefónico")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }
        public ReservationStatus Status { get; set; }
        [Required]
        [Display(Name = "Motivo")]
        public string Reason { get; set; }
    }
}