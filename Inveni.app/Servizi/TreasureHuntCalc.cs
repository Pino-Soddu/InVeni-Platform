using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core
{
    public static class TreasureHuntCalc
    {
        public static Entities.TreasureHuntCalcResult Compute(Entities.TreasureHuntItem treasureHuntItem, Models.ItemItinerario itemItinerario, CLLocation location, CLHeading heading, float? inclination)
        {
            Entities.TreasureHuntCalcResult result = new Entities.TreasureHuntCalcResult();

            if (treasureHuntItem.IndizioEnigma == null)
            {
                result.Message = "IndizioEnigma null nel gruppo corrente";
                return result;
            }

            if (treasureHuntItem.Caccia == null)
            {
                result.Message = "Caccia null nel gruppo corrente";
                return result;
            }


            if (treasureHuntItem.IndizioEnigma.EnigmaType != Models.EnigmaType.ENIGMA)
            {
                result.Message = "non è un'enigma ma un indizio o un aiuto";
                return result;
            }

            if (location == null)
            {
                result.Message = "Latest location is null";
                return result;
            }

            if (inclination == null)
            {
                result.Message = "LatestAccelerometerData is null";
                return result;
            }

            //Models.ItemItinerario itemItinerario = treasureHuntItem.ItemsItinerario.Where(x => x.schedaCode == treasureHuntItem.IndizioEnigma.schedaCode).FirstOrDefault();

            if (itemItinerario == null)
            {
                result.Message = "ItemItinerario non trovato per scheda specificata";
                return result;
            }


            result.Angle = CalcolaAngoloRetta(location.Coordinate.Latitude, location.Coordinate.Longitude, treasureHuntItem.IndizioEnigma.lat, treasureHuntItem.IndizioEnigma.lon);
            result.Location = location;
            result.TreasureHuntLocation = new CLLocation(treasureHuntItem.IndizioEnigma.lat, treasureHuntItem.IndizioEnigma.lon);
            result.Heading = heading.MagneticHeading;
            result.HuntPrecision = itemItinerario.PrecisioneCaccia;
            result.Inclination = inclination.Value;
            result.InclinationFrom = itemItinerario.InclinazioneDa;
            result.InclinationTo = itemItinerario.InclinazioneA;

            double minAngle = result.Angle - (result.HuntPrecision.HasValue ? result.HuntPrecision.Value : 10);
            double maxAngle = result.Angle + (result.HuntPrecision.HasValue ? result.HuntPrecision.Value : 10);

            if (minAngle < 0)
            {
                double tmpMin = 360 - Math.Abs(minAngle);
                double tmpMax = 360;
                minAngle = 0;

                if (result.Heading >= tmpMin && result.Heading <= tmpMax)
                {
                    result.IsSuccess = true;
                }

            }
            else if (maxAngle > 360)
            {
                double tmpMin = 0;
                double tmpMax = maxAngle - 360;
                maxAngle = 360;

                if (result.Heading >= tmpMin && result.Heading <= tmpMax)
                {
                    result.IsSuccess = true;
                }
            }

            if (!result.IsSuccess)
            {
                if (result.Heading >= minAngle && result.Heading <= maxAngle)
                {
                    result.IsSuccess = true;
                }
            }

            if (result.IsSuccess)
            {
                result.IsSuccess = result.Inclination >= result.InclinationFrom && result.Inclination <= result.InclinationTo;
            }

            return result;
        }

        static public double CalcolaAngoloRetta(double lat1, double lon1, double lat2, double lon2)
        {
            long lat1HWS = ConvertiCoordToHWS(lat1);
            long lon1HWS = ConvertiCoordToHWS(lon1);
            long lat2HWS = ConvertiCoordToHWS(lat2);
            long lon2HWS = ConvertiCoordToHWS(lon2);

            return CalcolaAngoloRetta(lat1HWS, lon1HWS, lat2HWS, lon2HWS);
        }
        static public double CalcolaAngoloRetta(long lat1, long lon1, long lat2, long lon2)
        {
            double risultato = 0;
            long quadrante = 0;
            if (lat2 > lat1 && lon2 > lon1)
                quadrante = 1;
            else if (lat2 < lat1 && lon2 > lon1)
                quadrante = 2;
            else if (lat2 < lat1 && lon2 < lon1)
                quadrante = 3;
            else if (lat2 > lat1 && lon2 < lon1)
                quadrante = 4;

            double CoeffAngolare = (double)(lat2 - lat1) / (double)(lon2 - lon1);
            double AngoloRetta = Math.Abs(Math.Atan(CoeffAngolare) * 180 / Math.PI);
            if (quadrante == 1)
                risultato = 90 - AngoloRetta;
            else if (quadrante == 2)
                risultato = 90 + AngoloRetta;
            else if (quadrante == 3)
                risultato = 270 - AngoloRetta;
            else if (quadrante == 4)
                risultato = 270 + AngoloRetta;
            else
            {
                if (lat2 < lat1 && lon2 == lon1)
                    risultato = 0;
                else if (lat2 == lat1 && lon2 > lon1)
                    risultato = 90;
                else if (lat2 > lat1 && lon2 == lon1)
                    risultato = 180;
                else if (lat2 == lat1 && lon2 < lon1)
                    risultato = 270;
            }
            return risultato;
        }

        static public long ConvertiCoordToHWS(double Coordinata)
        {
            double Conversione;

            Conversione = -1;

            double R2D = 180 / Math.PI;
            Conversione = Coordinata / R2D;
            Conversione = Conversione * 100000000;

            return ((long)Conversione);
        }

        static public double ConvertiCoordFromHWS(long CoordinataHWS)
        {
            double Risposta = 0;

            if (CoordinataHWS != 0 && CoordinataHWS != -1)
            {
                double AppoCoordinata = CoordinataHWS;

                Risposta = (AppoCoordinata / 100000000) * (180 / Math.PI);
            }

            if (Risposta < -180 || Risposta > 180)
                Risposta = 0;
            return (Risposta);
        }

    }
}