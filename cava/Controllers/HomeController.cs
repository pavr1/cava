using cava.Custom.Common;
using cava.Custom.Notification;
using cava.Custom.Serialization;
using cava.Enums;
using cava.Models;
using cava.Models.Custom;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using static cava.Custom.Notification.EmailSender;

namespace cava.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmailSender _emailService;

        public int CustomStatic { get; private set; }

        public HomeController()
        {
            _emailService = new EmailSender();
        }

        public ActionResult Index()
        {
            var delivered = CreateDeliverer();

            return View(delivered);
        }

        private Deliverer CreateDeliverer()
        {
            var whatsapplink = ConfigurationManager.AppSettings["WhatsappLink"];
            var whatsappPhone = ConfigurationManager.AppSettings["WhatsappPhone"];
            var whatsappDefaultMessage = ConfigurationManager.AppSettings["WhatsappMessage"];


            var delivered = new Deliverer
            {
                WhatsappUrl = whatsapplink + whatsappPhone + (!string.IsNullOrEmpty(whatsappDefaultMessage) ? "?text=" + whatsappDefaultMessage : string.Empty),
                FacebookLink = ConfigurationManager.AppSettings["FacebookLink"],
                InstagramLink = ConfigurationManager.AppSettings["InstagramLink"],
                Schedules = ConfigurationManager.AppSettings["Schedules"].ToUpper().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                BriefDescription = ConfigurationManager.AppSettings["BriefDescription"].ToUpper(),
                ReservationReasons = ConfigurationManager.AppSettings["ReservationReasons"].ToUpper().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                Slides = GetSlides()
            };

            return delivered;
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
            var delivered = CreateDeliverer();

            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Experience.ToString(), delivered);
        }

        public string Reservation()
        {
            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Reservation.ToString(), null);
        }

        public string Login()
        {
            return Serializer.RenderViewToString(this.ControllerContext, CommonObjects.Actions.Login.ToString(), null);
        }

        [HttpPost]
        public async Task<int> CreateReservation(DateTime reservationDate, int numberOfPeople, string reserverFirstName, string reserverLastName, DateTime? DOB, string phone, string email, string reason)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Task.Run(async () =>
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
                                Status = Enums.ReservationStatus.Active,
                                Reason = reason
                            };

                            db.Reservations.Add(reservation);
                            db.SaveChanges();

                            //Reservation email sent to admin
                            var msg = new CustomIdentityMessage
                            {
                                Body = string.Format(CommonObjects._RESERVATION_BODY,
                                reserverFirstName + " " + reserverLastName, numberOfPeople, reservationDate.ToString(CommonObjects._DATE_FORMAT_1) + " " + CommonObjects._A_LAS + " " + reservationDate.ToShortTimeString(), email, phone).ToUpper(),
                                Destination = WebConfigurationManager.AppSettings[CommonObjects._CAVA_ADMIN_EMAIL_KEY],
                                Subject = CommonObjects._NEW_RESERVATION_SUBJECT,
                                Bcc = WebConfigurationManager.AppSettings[CommonObjects._SUPPORT_ADMIN_EMAIL_KEY]
                            };

                            _emailService.SendAsync(msg);

                            //Confirmation email sent to customer
                            msg = new CustomIdentityMessage
                            {
                                Body = string.Format(CommonObjects._RESERVATION_EMAIL_BODY,
                                numberOfPeople, reservationDate.ToString(CommonObjects._DATE_FORMAT_1) + " " + CommonObjects._A_LAS + " " + reservationDate.ToShortTimeString(), email, phone).ToUpper(),
                                Destination = email,
                                Subject = CommonObjects._NEW_RESERVATION_SUBJECT2
                            };

                            _emailService.SendAsync(msg);
                        }
                    });

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

        private List<string> GetSlides()
        {
            var path = Server.MapPath("~/Content/v2.0/images/slides");

            string[] slides = Directory.GetFiles(path);
            List<string> slidePaths = new List<string>();

            foreach (string fileName in slides)
            {
                var splitValues = fileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

                var name = splitValues[splitValues.Length - 1].Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries)[0];

                slidePaths.Add(name);
            }

            return slidePaths;
        }
    }
}