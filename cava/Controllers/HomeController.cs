using cava.Custom.Notification;
using cava.Custom.Serialization;
using cava.Enums;
using cava.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;

namespace cava.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmailSender _emailService;

        public HomeController()
        {
            _emailService = new EmailSender();

            IdentityMessage msg = new IdentityMessage
            {
                Body = string.Format("<html><head></head><body>{0} HA REALIZADO UNA RESERVA DE {1} PERSONA(S) PARA EL {2}. <br /> INFORMACIÓN DE USUARIO: <br /> - CORREO ELECTRÓNICO: {3} <br /> - TELÉFONO: {4} </body></html>",
                            "PABLO VILLALOBOS", 2, new DateTime().ToString("dd/MM/yyyy") + " a las " + new DateTime().ToShortTimeString(), "pavr1@hotmail.com", "8844-3317"),
                Destination = WebConfigurationManager.AppSettings["adminEmail"],
                Subject = "NUEVA RESERVA CREADA"
            };

            _emailService.SendAsync(msg);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string Bar()
        {
            return Serializer.RenderViewToString(this.ControllerContext, "Bar", null);
        }

        public string Kitchen()
        {
            return Serializer.RenderViewToString(this.ControllerContext, "Kitchen", null);
        }

        public string Experience()
        {
            return Serializer.RenderViewToString(this.ControllerContext, "Experience", null);
        }

        public string Reservation()
        {
            return Serializer.RenderViewToString(this.ControllerContext, "Reservation", null);
        }

        public string Login()
        {
            return Serializer.RenderViewToString(this.ControllerContext, "Login", null);
        }

        [HttpPost]
        public int CreateReservation(DateTime reservationDate, int numberOfPeople, string reserverFirstName, string reserverLastName, DateTime? DOB, string phone, string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        var reservation = new Reservation
                        {
                            DOB = DOB,
                            Email = email,
                            NumberOfPeople = numberOfPeople,
                            Phone = phone,
                            ReservationDate = reservationDate,
                            ReserverFirstName = reserverFirstName,
                            ReserverLastName = reserverLastName,
                            Status = Enums.ReservationStatus.Active
                        };

                        db.Reservations.Add(reservation);
                        db.SaveChanges();

                        IdentityMessage msg = new IdentityMessage
                        {
                            Body = string.Format("<html><head></head><body>{0} HA REALIZADO UNA RESERVA DE {1} PERSONA(S) PARA EL {2}. <br /> INFORMACIÓN DE USUARIO: <br /> - CORREO ELECTRÓNICO: {3} <br /> - TELÉFONO: {4} </body></html>",
                            reserverFirstName + " " + reserverLastName, numberOfPeople, reservationDate.ToString("dd/MM/yyyy") + " a las " + reservationDate.ToShortTimeString(), email, phone),
                            Destination = WebConfigurationManager.AppSettings["adminEmail"],
                            Subject = "NUEVA RESERVA CREADA"
                        };

                        _emailService.SendAsync(msg);

                        msg = new IdentityMessage
                        {
                            Body = string.Format("<html><head></head><body>USTED HA REALIZADO UNA RESERVA EN WWW.CAVARESTOBAR.COM DE {0} PERSONA(S) PARA EL {1}. </body></html>",
                            numberOfPeople, reservationDate.ToString("dd/MM/yyyy") + " a las " + reservationDate.ToShortTimeString(), email, phone),
                            Destination = email,
                            Subject = "RESERVACIONES CAVA RESTOBAR"
                        };

                        _emailService.SendAsync(msg);
                    }

                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                //log ex
                return -1;
            }
        }

        public ActionResult ReservationHandler(string msg)
        {
            ViewBag.Message = msg;

            //var script = "$('#txt-email').notify('¡" + msg + "!', { position: 'top', className: 'warn' });";

            //var scriptManager = ScriptManager.GetCurrent(this);
            //new ClientScriptManager().RegisterStartupScript(script);

            return View();
        }

        public string RetrieveReservations(string status, string name, string email, string phone, DateTime? date)
        {
            var statusEnum = ReservationStatus.Active;

            if (date == null)
            {
                date = DateTime.Now;
            }

            if (!Enum.TryParse(status, out statusEnum))
            {
                //throw error (not likely to happen)
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var activeReservations = db.Reservations.Where(x => x.Status == statusEnum && System.Data.Entity.DbFunctions.TruncateTime(x.ReservationDate) == ((DateTime)date).Date);

                if (name != null && !name.Trim().Equals(string.Empty))
                {
                    activeReservations = activeReservations.Where(x => name.Contains(x.ReserverFirstName) || name.Contains(x.ReserverLastName));
                }

                if (email != null && !email.Trim().Equals(string.Empty))
                {
                    activeReservations = activeReservations.Where(x => x.Email.Contains(email));
                }

                if (phone != null && !phone.Trim().Equals(string.Empty))
                {
                    activeReservations = activeReservations.Where(x => x.Phone.Contains(phone));
                }

                var list = activeReservations.ToList();

                return Serializer.RenderViewToString(this.ControllerContext, "ReservationRetrieve", list);
            }
        }

        [HttpPost]
        public int UpdateReservationStatus(int reservationId, string status)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var reservation = db.Reservations.SingleOrDefault(x => x.ReservationId == reservationId);

                    if (reservation == null)
                    {
                        return -1;
                    }

                    var reservationStatus = ReservationStatus.Active;

                    if (!Enum.TryParse(status, out reservationStatus))
                    {
                        return -2;
                    }

                    reservation.Status = reservationStatus;

                    db.Entry(reservation).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    return 1;
                }
                catch (Exception ex)
                {
                    //log
                    return -3;
                }
            }
        }
    }
}