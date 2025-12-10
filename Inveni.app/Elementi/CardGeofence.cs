using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core.Entities
{
    public class GeofenceCard
    {
        public double CenterLat { get; set; }
        public double CenterLng { get; set; }
        public double Radius { get; set; }
        public string Name { get; set; }
        public double CurrentDistance { get; set; }
        public Models.Scheda Card { get; set; }
        public bool Playing { get; set; }
        public Mode StartPlayingMode { get; set; }
    }

    public class GeofenceCardComparer : IEqualityComparer<GeofenceCard>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(GeofenceCard x, GeofenceCard y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.Card._id == y.Card._id;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(GeofenceCard item)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(item, null)) return 0;

            //Calculate the hash code for the product.
            return item.Card._id.GetHashCode();
        }
    }
}