﻿using cava.Custom.Serialization;
using cava.Enums;
using cava.Models;
using System;
using System.Linq;
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

        public ActionResult ReservationHandler()
        {
            return View();
        }

        public string RetrieveReservations(string status, string name, string email, string phone, DateTime? date)
        {
            var statusEnum = ReservationStatus.Active;

            if(date == null)
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