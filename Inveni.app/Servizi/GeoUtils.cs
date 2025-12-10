using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core
{
    public class GeoUtils
    {
        public static List<Entities.GeofenceCard> GetTop20NearestCards(List<Models.Scheda> cards, double deviceLatitude, double deviceLongitude)
        {
            List<Entities.GeofenceCard> nearest = new List<Entities.GeofenceCard>();
            Geo.Coordinate devicePosition = new Geo.Coordinate(deviceLatitude, deviceLongitude);
            Geo.Geodesy.SpheroidCalculator calc = new Geo.Geodesy.SpheroidCalculator();
            
            foreach (var card in cards.Where(x => !x.IsTreasureHuntItem))
            {
                Entities.GeofenceCard item = new Entities.GeofenceCard();
                item.CenterLat = card.lat;
                item.CenterLng = card.lon;
                item.Radius = card.radius;
                item.Name = card.name;
                item.Card = card;
                item.StartPlayingMode = Mode.FOREGROUND;

                Geo.Coordinate coord = new Geo.Coordinate(item.CenterLat, item.CenterLng);
                item.CurrentDistance = calc.CalculateOrthodromicLine(devicePosition, coord).Distance.Value;

                nearest.Add(item);
            }

            return nearest.OrderBy(x => x.CurrentDistance).Take(20).ToList();
        }
    }
}