using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cava.Controllers
{
    public class BarController : Controller
    {
        // GET: Bar
        public ActionResult Index()
        {
            return View();
        }
    }
}