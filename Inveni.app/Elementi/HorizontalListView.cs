using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; }
        public CategoryType CategoryType { get; set; }
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public Category Category { get; set; }
        //public ItemType ItemType { get; set; }
    }

    public enum CategoryType
    {
        NOT_TO_BE_MISSED = 1,
        CARD = 2,
        TREASURE_HUNT = 3,
        ITINERARY = 4
    }

    //public enum ItemType
    //{
    //    NOT_TO_BE_MISSED = 1,
    //    CARD = 2,
    //    ITINERARY = 3
    //}
}