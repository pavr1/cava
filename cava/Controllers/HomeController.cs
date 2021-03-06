using cava.Custom.Common;
using cava.Custom.Notification;
using cava.Custom.Serialization;
using cava.Enums;
using cava.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;

namespace cava.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmailSender _emailService;

        public int CustomStatic { get; private set; }

        public HomeController()
        {
            _emailService = new EmailSender();

            //IdentityMessage msg = new IdentityMessage
            //{
            //    Body = string.Format("<html><head></head><body>{0} HA REALIZADO UNA RESERVA DE {1} PERSONA(S) PARA EL {2}. <br /> INFORMACIÓN DE USUARIO: <br /> - CORREO ELECTRÓNICO: {3} <br /> - TELÉFONO: {4} </body></html>",
            //                "PABLO VILLALOBOS", 2, new DateTime().ToString(CommonObjects._DATE_FORMAT_1) + CommonObjects._A_LAS + new DateTime().ToShortTimeString(), "pavr1@hotmail.com", "8844-3317"),
            //    Destination = WebConfigurationManager.AppSettings[CommonObjects._CONFIG_ADMIN_EMAIL],
            //    Subject = CommonObjects._NEW_RESERVATION_CREATED
            //};

            //_emailService.SendAsync(msg);
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
            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Bar.ToString(), null);
        }

        public string Kitchen()
        {
            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Kitchen.ToString(), null);
        }

        public string Experience()
        {
            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Experience.ToString(), null);
        }

        public string Reservation()
        {
            var whatsapplink = ConfigurationManager.AppSettings["WhatsappLink"];
            var whatsappPhone = ConfigurationManager.AppSettings["WhatsappPhone"];
            var whatsappDefaultMessage = ConfigurationManager.AppSettings["WhatsappMessage"];

            var whatsappUrl = whatsapplink + whatsappPhone + (!string.IsNullOrEmpty(whatsappDefaultMessage) ? "?text=" + whatsappDefaultMessage : string.Empty);
            
            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Reservation.ToString(), new Reservation { Whatsapp = whatsappUrl });
        }

        public string Login()
        {
            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Login.ToString(), null);
        }

        [HttpPost]
        public async Task<int> CreateReservation(DateTime reservationDate, int numberOfPeople, string reserverFirstName, string reserverLastName, DateTime? DOB, string phone, string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        reserverFirstName = Server.HtmlEncode(reserverFirstName);
                        reserverLastName = Server.HtmlEncode(reserverLastName);
                        phone = Server.HtmlEncode(phone);
                        email = Server.HtmlEncode(email);

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

                        //Reservation email sent to admin
                        IdentityMessage msg = new IdentityMessage
                        {
                            Body = string.Format(CommonObjects._RESERVATION_BODY,
                            reserverFirstName + " " + reserverLastName, numberOfPeople, reservationDate.ToString(CommonObjects._DATE_FORMAT_1) + CommonObjects._A_LAS + reservationDate.ToShortTimeString(), email, phone).ToUpper(),
                            Destination = WebConfigurationManager.AppSettings[CommonObjects._CONFIG_ADMIN_EMAIL_KEY] + "," + WebConfigurationManager.AppSettings[CommonObjects._CAVA_ADMIN_EMAIL_KEY],
                            Subject = CommonObjects._NEW_RESERVATION_SUBJECT
                        };

                        await _emailService.SendAsync(msg);

                        //Confirmation email sent to customer
                        msg = new IdentityMessage
                        {
                            Body = string.Format(CommonObjects._RESERVATION_EMAIL_BODY,
                            numberOfPeople, reservationDate.ToString(CommonObjects._DATE_FORMAT_1) + CommonObjects._A_LAS + reservationDate.ToShortTimeString(), email, phone).ToUpper(),
                            Destination = email,
                            Subject = CommonObjects._NEW_RESERVATION_SUBJECT2
                        };

                        await _emailService.SendAsync(msg);
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
                Log log = new Log()
                {
                    Date = DateTime.Now,
                    LogGuid = Guid.NewGuid(),
                    MessageDetail = ex.Message + " | " + ex.InnerException?.Message,
                    MessageType = CommonObjects._ERROR,
                    Line = CommonStatic.GetCurrentLine(),
                    Method = CommonStatic.GetCurrentMethod()
                };

                new LogController().LogMessageAsync(log);

                return -1;
            }
        }

        public ActionResult ReservationHandler(string msg)
        {
            ViewBag.Message = msg;

            return View();
        }

        public string RetrieveReservations(string status, string name, string email, string phone, DateTime? date)
        {
            try
            {
                var statusEnum = ReservationStatus.Active;

                status = Server.HtmlEncode(status).Trim();
                name = Server.HtmlEncode(name).Trim();
                email = Server.HtmlEncode(email).Trim();
                phone = Server.HtmlEncode(phone).Trim();

                if (date == null)
                {
                    date = DateTime.Now;
                }

                if (!Enum.TryParse(status, out statusEnum))
                {
                    throw new Exception("Invalid enum status: " + status);
                    //throw error (not likely to happen)
                }

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var activeReservations = db.Reservations.Where(x => x.Status == statusEnum && System.Data.Entity.DbFunctions.TruncateTime(x.ReservationDate) == ((DateTime)date).Date);

                    if (name != null && !name.Trim().Equals(string.Empty))
                    {
                        activeReservations = activeReservations.Where(x => x.ReserverFirstName.Contains(name) || x.ReserverLastName.Contains(name));
                    }

                    if (email != null && !email.Trim().Equals(string.Empty))
                    {
                        activeReservations = activeReservations.Where(x => x.Email.Contains(email));
                    }

                    if (phone != null && !phone.Trim().Equals(string.Empty))
                    {
                        activeReservations = activeReservations.Where(x => x.Phone.Contains(phone));
                    }

                    var list = activeReservations.OrderBy(o => o.ReservationDate).ToList();

                    return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.ReservationRetrieve.ToString(), list);
                }

            }
            catch (Exception ex)
            {
                Log log = new Log()
                {
                    Date = DateTime.Now,
                    LogGuid = Guid.NewGuid(),
                    MessageDetail = ex.Message + " | " + ex.InnerException?.Message,
                    MessageType = CommonObjects._ERROR,
                    Line = CommonStatic.GetCurrentLine(),
                    Method = CommonStatic.GetCurrentMethod()
                };

                new LogController().LogMessageAsync(log);

                return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.ReservationRetrieve.ToString(), new List<Reservation>());
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
                    Log log = new Log()
                    {
                        Date = DateTime.Now,
                        LogGuid = Guid.NewGuid(),
                        MessageDetail = ex.Message + " | " + ex.InnerException?.Message,
                        MessageType = CommonObjects._ERROR,
                        Line = CommonStatic.GetCurrentLine(),
                        Method = CommonStatic.GetCurrentMethod()
                    };

                    new LogController().LogMessageAsync(log);

                    return -3;
                }
            }
        }
    }
}