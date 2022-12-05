using System;
using System.Collections.Generic;
using System.Text;
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
        [Column("name")]
        public string Name { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Indexed]
        [Column("last_filled_by_id")]
        public int? LastFilledById { get; set; } = null;

        [Ignore]
        public AppUser? LastFilledBy { get; set; }

        [OneToMany]
        public List<FridgeItem> FridgeItems { get; set; } = new();

        [Ignore]
        public virtual int FridgeItemsCount => FridgeItems.Count;
    }
}
