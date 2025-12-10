using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core.Entities
{
    public class TreasureHuntCalcResult
    {
        public CLLocation Location { get; set; }
        public CLLocation TreasureHuntLocation { get; set; }
        public double Angle { get; set; }
        public double Heading { get; set; }
        public int? HuntPrecision { get; set; }
        public float Inclination { get; set; }
        public double? InclinationFrom { get; set; }
        public double? InclinationTo { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}