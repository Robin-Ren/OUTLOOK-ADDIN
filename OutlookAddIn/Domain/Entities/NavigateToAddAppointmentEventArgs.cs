using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class NavigateToAddAppointmentEventArgs : EventArgs
    {
        public int FacilityID { get; set; }
        public DateTime StartTimeslot { get; set; }
        public DateTime EndTimeslot { get; set; }
    }

    public class SaveBookingRequestArgs
    {
        public Facility facility { get; set; }
        public string paymentType { get; set; } = "COMPANY";
        public string requestRemark { get; set; }
        public List<BookingDetail> bookingDetails { get; set; }
    }
}
