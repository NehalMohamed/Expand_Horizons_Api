using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITravelApp.Data.Models.Bookings
{
    public class ConfirmBookingReq
    {
        public long? booking_id { get; set; }
        public string? client_id { get; set; }
        public string? lang_code { get; set; }
        public string? ClientEmail { get; set; } = "";
    }

    public class ConfirmBookingPosReq
    {
        public long? booking_id { get; set; }
        public string? client_id { get; set; }
        public string? ClientEmail { get; set; }
        public string? lang_code { get; set; }
    }
}
