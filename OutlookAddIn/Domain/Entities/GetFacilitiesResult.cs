using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class GetFacilitiesResult
    {
        public FacilitySearchCriteria facilitySearchCriteria { get; set; }
        public List<Facility> entities { get; set; }
    }
}
