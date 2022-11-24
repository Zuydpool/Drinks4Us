using System;
using System.Collections.Generic;
using System.Text;

namespace Drinks4Us.Models
{
    public class Fridge
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public List<FridgeItem> FridgeItems { get; set; }
    }
}
