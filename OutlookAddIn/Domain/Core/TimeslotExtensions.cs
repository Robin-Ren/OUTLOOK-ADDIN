using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public static class TimeslotExtensions
    {
        public static bool IsAdjacentAfter(this TimeSlot instance, TimeSlot previousTimeslot)
        {
            if (!instance.available || !previousTimeslot.available)
                return false;

            if (instance.from != previousTimeslot.to)
                return false;

            return true;
        }
    }
}
