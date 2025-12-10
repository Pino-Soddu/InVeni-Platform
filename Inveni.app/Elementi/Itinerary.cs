using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Inveni.app.Elementi
{
    public enum ItineratyType
    {
        STANDARD,
        TREASURE_HUNT
    }

    public class Itinerary
    {
        public string Name { get; set; }
        public ItineratyType ItineratyType { get; set; }
        public int IdGioco { get; set; }
    }
}