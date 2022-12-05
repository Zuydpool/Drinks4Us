using System;
using Drinks4Us.Exceptions;
using Drinks4Us.Extensions;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Drinks4Us.Models
{
    [Table("fridge_items")]
    public class FridgeItem
    {
        private const int WeekLimit = 10;

        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("purchase_date")]
        public DateTime PurchaseDate { get; set; }

        [Column("expire_date")]
        public DateTime? ExpireDate { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Indexed]
        [Column("fridge_id")]
        [ForeignKey(typeof(Fridge))]
        public int FridgeId { get; set; }

        [ManyToOne]
        public Fridge? Fridge { get; set; }

        [Ignore]
        public string Expiring => (IsExpiringSoon(DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(3)), false) ? GetExpiringSoonMessage(DateTime.Today) : (IsExpired(DateTime.Today, false) ? GetExpiredMessage(DateTime.Today) : "Will never expire"));

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

        public string GetExpiringSoonMessage(DateTime now)
        {
            var todayTime = new DateTimeOffset(now).ToUnixTimeMilliseconds();
            var expiringTime = new DateTimeOffset((DateTime)ExpireDate!).ToUnixTimeMilliseconds();
            
            var days = GetDaysToTime(todayTime, expiringTime);


            if (!(days >= 0L)) throw new IllegalArgumentException("days must be 0 or higher");

            switch (days)
            {
                case 0L:
                    return "expires today";
                case < 7L:
                    return $"will expire in {days} {(days == 1 ? "day" : "days")}";
                default:
                {
                    var weeks = days / 7L;
                    return weeks < WeekLimit
                        ? $"will expire in {weeks} {(weeks == 1 ? "week" : "weeks")}"
                        : "will not expire for a long time";
                }
            }
        }

        public string GetExpiredMessage(DateTime now)
        {
            var todayTime = new DateTimeOffset(now).ToUnixTimeMilliseconds();
            var expiringTime = new DateTimeOffset((DateTime)ExpireDate!).ToUnixTimeMilliseconds();

            var days = GetDaysToTime(expiringTime, todayTime);

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
                    return weeks < WeekLimit
                        ? $"expired {weeks} {(weeks == 1 ? "week" : "weeks")}"
                        : "expired a long time ago";
                }
            }
        }

        public bool IsExpiringSoon(DateTime date, DateTime later, bool countSameDayAsExpired)
        {
            var expireTime = ExpireDate;
            if (expireTime == null)
            {
                return false;
            }

            if (IsExpired(date, countSameDayAsExpired))
            {
                return false;
            }

            var expiration = expireTime?.CleanMidnight();
            var midnightToday = date.CleanMidnight();
            var midnightLater = later.CleanMidnight();

            return expiration > midnightToday || expiration == midnightToday || expiration == midnightLater;
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