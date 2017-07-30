using System;
using System.Globalization;

namespace Flamtap.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Get the week number of the year for this DateTime, according to the ISO 8601 standard.
        /// </summary>
        /// <remarks>
        ///     Presumes that weeks start with Monday. Week 1 is the 1st week of the year with
        ///     a thursday in it. If the time is a Monday, Tuesday, or Wednesday, then it will
        ///     be the same week number as whatever Thursday, Friday or Saturday are.
        /// </remarks>
        /// <see cref="https://blogs.msdn.microsoft.com/shawnste/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net/" />
        /// <param name="time">The DateTime.</param>
        /// <returns>The number or the week that the DateTime falls in.</returns>
        public static int GetIso8601WeekOfYear(this DateTime time)
        {
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            
            DayOfWeek day = calendar.GetDayOfWeek(time);

            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
                time = time.AddDays(3);
            
            return calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
