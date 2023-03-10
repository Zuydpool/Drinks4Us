using System;
using Drinks4Us.Exceptions;
using Drinks4Us.Extensions;
using SQLite;
using SQLiteNetExtensions.Attributes;

// TODO: Add Quantity type
// TODO: Add item state (opened, closed, etc)
namespace Drinks4Us.Models
{
    [Table("fridge_items")]
    public class FridgeItem
    {
        private const int WeekLimit = 10;

        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")] public string Name { get; set; }

        [Column("quantity")] public int Quantity { get; set; }

        [Column("purchase_date")] public DateTime PurchaseDate { get; set; }

        [Column("expire_date")] public DateTime? ExpireDate { get; set; }

        [Column("image_url")] public string ImageUrl { get; set; }

        [Indexed]
        [Column("fridge_id")]
        [ForeignKey(typeof(Fridge))]
        public int FridgeId { get; set; }

        [ManyToOne] public Fridge? Fridge { get; set; }

        [Ignore]
        public string Expiring => (IsExpiringSoon(DateTime.Today, DateTime.Today.Add(TimeSpan.FromDays(3)), false)
            ? GetExpiringSoonMessage(DateTime.Today)
            : (IsExpired(DateTime.Today, false) ? GetExpiredMessage(DateTime.Today) : "Will never expire"));

        public FridgeItemBuilder ToBuilder() => new(this);

        public override string ToString() => "FridgeItem{Id=" + Id + ", Name=" + Name + ", Quantity=" + Quantity + ", PurchaseDate=" + PurchaseDate + ", ExpireDate=" + ExpireDate + "}";

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

    public class FridgeItemBuilder
    {
        private int _id;
        private string _name;
        private int _quantity;
        private DateTime _purchaseDate;
        private DateTime? _expireDate;
        private string _imageUrl;

        private Fridge? _fridge;

        protected FridgeItemBuilder()
        {
            // Empty constructor
        }

        public FridgeItemBuilder(FridgeItem fridgeItem)
        {
            this._id = fridgeItem.Id;
            this._name = fridgeItem.Name;
            this._quantity = fridgeItem.Quantity;
            this._purchaseDate = fridgeItem.PurchaseDate;
            this._expireDate = fridgeItem.ExpireDate;
            this._imageUrl = fridgeItem.ImageUrl;
            this._fridge = fridgeItem.Fridge;
        }

        public FridgeItem Build()
        {
            if (_imageUrl == null)
            {
                throw new IllegalArgumentException("You need to take or select a image!");
            }

            return new FridgeItem
            {
                Id = this._id,
                Name = this._name,
                Quantity = this._quantity,
                PurchaseDate = this._purchaseDate,
                ExpireDate = this._expireDate,
                ImageUrl = this._imageUrl,
                Fridge = this._fridge,
                FridgeId = this._fridge?.Id ?? 0
            };
        }

        public FridgeItemBuilder Id(int id)
        {
            this._id = id;
            return this;
        }

        public FridgeItemBuilder Name(string name)
        {
            this._name = name;
            return this;
        }

        public FridgeItemBuilder Quantity(int quantity)
        {
            this._quantity = quantity;
            return this;
        }

        public FridgeItemBuilder PurchaseDate(DateTime purchaseDate)
        {
            this._purchaseDate = purchaseDate;
            return this;
        }

        public FridgeItemBuilder ExpireDate(DateTime expireDate)
        {
            this._expireDate = expireDate;
            return this;
        }

        public FridgeItemBuilder ImageUrl(string imageUrl)
        {
            this._imageUrl = imageUrl;
            return this;
        }

        public FridgeItemBuilder Fridge(Fridge fridge)
        {
            this._fridge = fridge;
            return this;
        }
    }
}