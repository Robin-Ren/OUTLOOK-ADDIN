using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookAddIn.CustomScheduler.Model;

namespace OutlookAddIn.CustomScheduler.Controls
{
    public static class Filters
    {
        public static IEnumerable<Appointment> ByDate(this IEnumerable<Appointment> appointments, DateTime date)
        {
            var app = from a in appointments
                      where a.StartTime.ToShortDateString() == date.ToShortDateString()
                      select a;
            return app;
        }
    }
}
