using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drinks4Us.Exceptions;
using Drinks4Us.Extensions;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Drinks4Us.Models
{
    [Table("fridges")]
    public class Fridge
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")] public string Name { get; set; }
        [Column("image_url")] public string ImageUrl { get; set; }

        [Indexed]
        [Column("last_filled_by_id")]
        public int? LastFilledById { get; set; } = null;

        [Ignore] public AppUser? LastFilledBy { get; set; }

        [OneToMany] public List<FridgeItem> FridgeItems { get; set; } = new();

        [Ignore] public virtual int FridgeItemsCount => FridgeItems.Count;

        public bool NeedsReplenish
        {
            get
            {
                if (FridgeItemsCount == 0) return false;
                return FridgeItems.Count(fridgeItem => fridgeItem.Quantity <= 1) >= 2;
            }
        }

        public string NeedsReplenishText => NeedsReplenish ? "Needs replenish" : "";

        public async Task<AppUser?> GetRandomReplenisher()
        {
            var appUsers = await App.GetInstance().Storage.Dao.AppUsersDao.GetAll();
            var replenisher = appUsers.Count == 0 ? null : appUsers.GetRandom();
            while (replenisher != null && LastFilledBy == replenisher)
            {
                replenisher = appUsers.GetRandom();
            }

            return replenisher;
        }

        public FridgeBuilder ToBuilder() => new(this);
    }

    public class FridgeBuilder
    {
        private int _id;
        private string _name;
        private string _imageUrl;

        private AppUser? _lastFilledBy;

        private List<FridgeItem> _FridgeItems = new();

        protected FridgeBuilder()
        {
            // Empty constructor
        }

        public FridgeBuilder(Fridge fridge)
        {
            this._id = fridge.Id;
            this._name = fridge.Name;
            this._imageUrl = fridge.ImageUrl;
            this._lastFilledBy = fridge.LastFilledBy;
            this._FridgeItems = fridge.FridgeItems;
        }

        public Fridge Build()
        {
            if (_imageUrl == null)
            {
                throw new IllegalArgumentException("You need to take or select a image!");
            }

            return new Fridge
            {
                Id = this._id,
                Name = this._name,
                ImageUrl = this._imageUrl,
                LastFilledBy = this._lastFilledBy,
                FridgeItems = this._FridgeItems
            };
        }

        public FridgeBuilder Id(int id)
        {
            this._id = id;
            return this;
        }

        public FridgeBuilder Name(string name)
        {
            this._name = name;
            return this;
        }

        public FridgeBuilder ImageUrl(string imageUrl)
        {
            this._imageUrl = imageUrl;
            return this;
        }

        public FridgeBuilder LastFilledBy(AppUser lastFilledBy)
        {
            this._lastFilledBy = lastFilledBy;
            return this;
        }

        public FridgeBuilder FridgeItems(List<FridgeItem> fridgeItems)
        {
            this._FridgeItems = fridgeItems;
            return this;
        }

        public FridgeBuilder AddFridgeItem(FridgeItem item)
        {
            this._FridgeItems.Add(item);
            return this;
        }
    }
}