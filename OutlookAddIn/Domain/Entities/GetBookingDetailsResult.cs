using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class GetBookingDetailsResult
    {
        public FacilityBookingDetailSearchCriteria facilityBookingDetailSearchCriteria { get; set; }
        public List<Entity> entities { get; set; }
    }
}
