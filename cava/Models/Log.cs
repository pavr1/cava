using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cava.Models
{
    public class Log
    {
        public int LogId { get; set; }
        public Guid LogGuid { get; set; }
        public string MessageType { get; set; }
        public string MessageDetail { get; set; }
        public DateTime Date { get; set; }
        public string Method { get; set; }
        public int Line { get; set; }
    }
}