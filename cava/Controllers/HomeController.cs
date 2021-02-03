using cava.Custom.Serialization;
using cava.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cava.Controllers
{
    public class HomeController : Controller
    {
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
    }
}