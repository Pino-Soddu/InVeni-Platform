using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core.Entities
{
    public class CustomLocation
    {
        public CLLocation Location { get; private set; }
        public DateTime Date { get; private set; }

        public CustomLocation(CLLocation location)
        {
            Location = location;
            Date = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            CustomLocation tmp = (CustomLocation)obj;
            return Location.Coordinate.Latitude.ToString("F5").Equals(tmp.Location.Coordinate.Latitude.ToString("F5"))
                && Location.Coordinate.Longitude.ToString("F5").Equals(tmp.Location.Coordinate.Longitude.ToString("F5"));
        }

        public override int GetHashCode()
        {
            return Location.GetHashCode() + Date.GetHashCode();
        }
    }
}