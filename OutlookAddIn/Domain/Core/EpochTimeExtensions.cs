using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public static class EpochTimeExtensions
    {
        /// <summary>
        /// Converts the given date value to epoch time.
        /// </summary>
        public static long ToUtcEpochTime(this DateTime dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ticks = date.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).Ticks;
            var ts = ticks / TimeSpan.TicksPerMillisecond;
            return ts;
        }

        /// <summary>
        /// Converts the given date value to epoch time.
        /// </summary>
        public static long ToLocalEpochTime(this DateTime dateTime)
        {
            var date = dateTime.ToLocalTime();
            var ticks = date.Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).Ticks;
            var ts = ticks / TimeSpan.TicksPerMillisecond;
            return ts;
        }

        /// <summary>
        /// Converts the given date value to epoch time.
        /// </summary>
        public static long ToSingaporeEpochTime(this DateTime dateTime)
        {
            var epochTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Singapore Standard Time");
            var ticks = dateTime.Ticks - epochTime.Ticks;
            var ts = ticks / TimeSpan.TicksPerMillisecond;
            return ts;
        }

        /// <summary>
        /// Converts the given date value to epoch time.
        /// </summary>
        public static long ToUtcEpochTime(this DateTimeOffset dateTime)
        {
            var date = dateTime.ToUniversalTime();
            var ticks = date.Ticks - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).Ticks;
            var ts = ticks / TimeSpan.TicksPerMillisecond;
            return ts;
        }

        /// <summary>
        /// Converts the given date value to epoch time.
        /// </summary>
        public static long ToLocalEpochTime(this DateTimeOffset dateTime)
        {
            var date = dateTime.ToLocalTime();
            var ticks = date.Ticks - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).ToLocalTime().Ticks;
            var ts = ticks / TimeSpan.TicksPerMillisecond;
            return ts;
        }

        /// <summary>
        /// Converts the given epoch time to a <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/> kind.
        /// </summary>
        public static DateTime ToUtcDateTimeFromEpoch(this long millisecond)
        {
            var timeInTicks = millisecond * TimeSpan.TicksPerMillisecond;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks);
        }

        /// <summary>
        /// Converts the given epoch time to a <see cref="DateTime"/> with <see cref="DateTimeKind.Utc"/> kind.
        /// </summary>
        public static DateTime ToSingaporeDateTimeFromEpoch(this long millisecond)
        {
            var timeInTicks = millisecond * TimeSpan.TicksPerMillisecond;
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(timeInTicks);
            return System.TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Singapore Standard Time");
        }

        /// <summary>
        /// Converts the given epoch time to a <see cref="DateTime"/> with <see cref="DateTimeKind.Local"/> kind.
        /// </summary>
        public static DateTime ToLocalDateTimeFromEpoch(this long millisecond)
        {
            var timeInTicks = millisecond * TimeSpan.TicksPerMillisecond;
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddTicks(timeInTicks);
        }

        /// <summary>
        /// Converts the given epoch time to a UTC <see cref="DateTimeOffset"/>.
        /// </summary>
        public static DateTimeOffset ToUtcDateTimeOffsetFromEpoch(this long millisecond)
        {
            var timeInTicks = millisecond * TimeSpan.TicksPerMillisecond;
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(timeInTicks);
        }

        /// <summary>
        /// Converts the given epoch time to a Local <see cref="DateTimeOffset"/>.
        /// </summary>
        public static DateTimeOffset ToLocalDateTimeOffsetFromEpoch(this long millisecond)
        {
            var timeInTicks = millisecond * TimeSpan.TicksPerMillisecond;
            return new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).ToLocalTime().AddTicks(timeInTicks);
        }
    }
}
