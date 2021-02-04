using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cava.Enums
{
    public enum ReservationStatus
    {
        //default value when created
        Active = 1,
        //reservation used and over
        Expired = 2,
        //reservation cancelled.
        Cancelled = 3,
        //reservation confirmed
        Confirmed = 4
    }
}