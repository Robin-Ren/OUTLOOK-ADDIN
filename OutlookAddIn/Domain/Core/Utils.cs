using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public static class Utils
    {
        public static DateTime ConvertUnixTicksToDateTime(long ticks)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddMilliseconds(ticks).ToLocalTime();

            return date;
        }
    }
}
