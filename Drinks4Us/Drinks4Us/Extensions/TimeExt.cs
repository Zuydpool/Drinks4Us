using System;

namespace Drinks4Us.Extensions
{
    public static class TimeExt
    {

        public static DateTime CleanMidnight(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }
    }
}