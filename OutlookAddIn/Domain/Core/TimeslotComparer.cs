using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class TimeslotComparer : IComparer<TimeSlot>
    {
        public int Compare(TimeSlot x1, TimeSlot x2)
        {
            if (x1.from > x2.from) return 1;
            if (x1.from == x2.from) return 0;
            return -1;
        }
    }
}
