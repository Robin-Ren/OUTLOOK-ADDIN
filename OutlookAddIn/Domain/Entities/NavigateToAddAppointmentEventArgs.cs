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
}
