using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inveni.App.Modelli;
using Inveni.App.Elementi;
using Microsoft.Maui.Devices.Sensors;

using Foundation;
using UIKit;

namespace Inveni.App.Servizi
{
    public class GeoUtils
    {
        public static List<Elementi.GeofenceCard> GetTop20NearestCards(List<Modelli.Scheda> cards, double deviceLatitude, double deviceLongitude)
        {
            List<Elementi.GeofenceCard> nearest = new List<Elementi.GeofenceCard>();
            Geo.Coordinate devicePosition = new Geo.Coordinate(deviceLatitude, deviceLongitude);
            Geo.Geodesy.SpheroidCalculator calc = new Geo.Geodesy.SpheroidCalculator();
            
            foreach (var card in cards.Where(x => !x.IsTreasureHuntItem))
            {
                Elementi.GeofenceCard item = new Elementi.GeofenceCard();
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