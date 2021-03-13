using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cava.Models.Custom
{
    public class Deliverer
    {
        public string WhatsappUrl { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public List<string> Schedules { get; set; }
        public string BriefDescription { get; set; }
        public List<string> Slides { get; set; }
        public List<string> ReservationReasons { get; set; }
    }
}