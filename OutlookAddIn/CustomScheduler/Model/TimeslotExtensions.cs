using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlookAddIn.CustomScheduler.Controls;

namespace OutlookAddIn.CustomScheduler.Model
{
    public static class TimeslotExtensions
    {
        public static bool IsAdjacentAfter(this CalendarTimeslotItem instance, CalendarTimeslotItem previousTimeslot)
        {
            if (instance.TimeslotDate.Date != previousTimeslot.TimeslotDate.Date)
                return false;

            if (instance.TimeslotStart != previousTimeslot.TimeslotEnd)
                return false;

            return true;
        }
    }
}
