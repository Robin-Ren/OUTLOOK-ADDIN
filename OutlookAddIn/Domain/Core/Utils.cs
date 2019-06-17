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

        public static Appointments ConvertBookingDetailsToAppointments(List<BookingDetail> bookingDetails)
        {
            if (bookingDetails == null || bookingDetails.Count == 0)
                return null;

            var appointments = new Appointments();

            foreach (var entity in bookingDetails)
            {
                if (entity.facilityBooking != null)
                {
                    var appointment = new Appointment
                    {
                        Subject = string.Format("{0} - {1}",
                                entity.facilityBooking.requestNo,
                                entity.facilityBooking.facility.name),
                        FacilityID = entity.facilityBooking.facility.id,
                        StartTime = entity.facilityBooking.requestedStartDate.Value.ToSingaporeDateTimeFromEpoch(),
                        EndTime = entity.facilityBooking.requestedEndDate.Value.ToSingaporeDateTimeFromEpoch(),
                    };
                    appointments.Add(appointment);
                }
            }

            return appointments;
        }
    }
}
