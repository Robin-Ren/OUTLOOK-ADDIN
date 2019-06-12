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
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = epoch.AddMilliseconds(ticks).ToLocalTime();

            return date;
        }

        public static long ConvertDateTimeToUnixTicks(DateTime date)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan span = (date.ToLocalTime() - epoch);

            return Convert.ToInt64(span.TotalMilliseconds);
        }
    }
}
