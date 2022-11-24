using System;
using System.Diagnostics;
using Drinks4Us.Exceptions;
using Drinks4Us.Extensions;

namespace Drinks4Us.Models
{
    public class FridgeItem
    {
        private const int WeekLimit = 10;

        public string Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public DateTime PurchaseDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string ImageUrl { get; set; }

        public void Consume()
        {
            if (Quantity > 0)
            {
                Quantity -= 1;
            }
        }

        private long GetDaysToTime(long nowTime, long expireTime)
        {
            var difference = expireTime - nowTime;
            var seconds = (difference / 1000L);
            var minutes = (seconds / 60L);
            var hours = (minutes / 60L);

            return (hours / 24L);
        }

        public string GetExpiredMessage(DateTime now)
        {
            var todayTime = new DateTimeOffset(now).ToUnixTimeMilliseconds();
            var expiringTime = new DateTimeOffset((DateTime) ExpireDate!).ToUnixTimeMilliseconds();

            var days = GetDaysToTime(expiringTime, todayTime);
            Debug.WriteLine("days: " + days);

            if (!(days >= 0L)) throw new IllegalArgumentException("days must be 0 or higher");

            switch (days)
            {
                case 0L:
                    return "expires today";
                case < 7L:
                    return $"expired {days} {(days == 1L ? "day" : "days")} ago";
                default:
                {
                    var weeks = days / 7L;
                    return weeks < WeekLimit ? $"expired {weeks} {(weeks == 1 ? "week" : "weeks")}" : "expired a long time ago";
                }
            }

        }

        public bool IsExpired(DateTime date, bool countSameDayAsExpired)
        {
            if (ExpireDate == null) return false;

            var expiration = ExpireDate?.CleanMidnight();

            var midnightToday = date.CleanMidnight();
            if (expiration < midnightToday) return true;

            if (countSameDayAsExpired)
            {
                return expiration == midnightToday;
            }

            return false;
        }
    }
}