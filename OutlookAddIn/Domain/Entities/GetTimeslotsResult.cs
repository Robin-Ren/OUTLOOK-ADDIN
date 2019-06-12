using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class GetTimeslotsResult
    {
        public List<TimeSlot> entities { get; set; }
        public TimeSlotSearchCriteria timeSlotSearchCriteria { get; set; }
    }

    public class TimeSlotSearchCriteria
    {
        public int facilityId { get; set; }
        public bool? isAvailable { get; set; }
        public long? date { get; set; }
    }
}
