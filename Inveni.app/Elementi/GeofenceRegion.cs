using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core.Entities
{
    public enum GeofenceRegionStatus
    {
        IN = 1,
        OUT = 2
    }

    public class GeofenceRegion
    {
        public event EventHandler<GeofenceRegion> OnStatusChanged;

        public CLCircularRegion CircularRegion { get; set; }
        public GeofenceCard GeofenceCard { get; set; }
        public GeofenceRegionStatus Status { get; private set; }
        public DateTime LastUpdate { get; set; }
        public bool Handled { get; set; }

        public GeofenceRegion()
        {
            Status = GeofenceRegionStatus.OUT;
            LastUpdate = DateTime.MinValue;
        }

        public void SetIn()
        {
            SetStatus(GeofenceRegionStatus.IN);
        }

        public void SetOut()
        {
            SetStatus(GeofenceRegionStatus.OUT);
        }

        private void SetStatus(GeofenceRegionStatus status)
        {
            if(LastUpdate.AddSeconds(3) >= DateTime.Now) return;

            Handled = false;
            Status = status;
            LastUpdate = DateTime.Now;
            if (OnStatusChanged != null)
                OnStatusChanged.Invoke(new object(), this);
        }
    }
}