using System;
using Inveni.App.Modelli;
using Inveni.App.Elementi;
using Microsoft.Maui.Devices.Sensors;

namespace Inveni.App.Servizi
{
    public static class CalcolatoreTesoro
    {
        public static RisultatoVerifica Compute(OggettoCaccia oggettoCaccia, ItemItinerario itemItinerario,
                                               Location posizioneUtente, double direzione, float? inclinazione)
        {
            var risultato = new RisultatoVerifica();

            if (oggettoCaccia.IndizioEnigma == null)
            {
                risultato.Messaggio = "IndizioEnigma null nel gruppo corrente";
                return risultato;
            }

            if (oggettoCaccia.Caccia == null)
            {
                risultato.Messaggio = "Caccia null nel gruppo corrente";
                return risultato;
            }

            if (oggettoCaccia.IndizioEnigma.EnigmaType != EnigmaType.ENIGMA)
            {
                risultato.Messaggio = "non è un'enigma ma un indizio o un aiuto";
                return risultato;
            }

            if (posizioneUtente == null)
            {
                risultato.Messaggio = "Posizione utente è null";
                return risultato;
            }

            if (inclinazione == null)
            {
                risultato.Messaggio = "Inclinazione è null";
                return risultato;
            }

            if (itemItinerario == null)
            {
                risultato.Messaggio = "ItemItinerario non trovato per scheda specificata";
                return risultato;
            }

            risultato.Angolo = CalcolaAngoloRetta(
                posizioneUtente.Latitude, posizioneUtente.Longitude,
                oggettoCaccia.IndizioEnigma.lat, oggettoCaccia.IndizioEnigma.lon);

            risultato.PosizioneUtente = posizioneUtente;
            risultato.PosizioneTesoro = new Location(oggettoCaccia.IndizioEnigma.lat, oggettoCaccia.IndizioEnigma.lon);

            risultato.Direzione = direzione;
            risultato.PrecisioneCaccia = itemItinerario.PrecisioneCaccia;
            risultato.Inclinazione = inclinazione.Value;
            risultato.InclinazioneDa = itemItinerario.InclinazioneDa;
            risultato.InclinazioneA = itemItinerario.InclinazioneA;

            double minAngle = risultato.Angolo - (risultato.PrecisioneCaccia ?? 10);
            double maxAngle = risultato.Angolo + (risultato.PrecisioneCaccia ?? 10);

            if (minAngle < 0)
            {
                double tmpMin = 360 - Math.Abs(minAngle);
                double tmpMax = 360;
                minAngle = 0;

                if (risultato.Direzione >= tmpMin && risultato.Direzione <= tmpMax)
                {
                    risultato.Successo = true;
                }
            }
            else if (maxAngle > 360)
            {
                double tmpMin = 0;
                double tmpMax = maxAngle - 360;
                maxAngle = 360;

                if (risultato.Direzione >= tmpMin && risultato.Direzione <= tmpMax)
                {
                    risultato.Successo = true;
                }
            }

            if (!risultato.Successo)
            {
                if (risultato.Direzione >= minAngle && risultato.Direzione <= maxAngle)
                {
                    risultato.Successo = true;
                }
            }

            if (risultato.Successo)
            {
                risultato.Successo = risultato.Inclinazione >= risultato.InclinazioneDa
                                  && risultato.Inclinazione <= risultato.InclinazioneA;
            }

            return risultato;
        }


        #region Metodi di calcolo Coordinate
        // Mantieni gli altri metodi (CalcolaAngoloRetta, ConvertiCoordToHWS, ConvertiCoordFromHWS) così come sono
        // Non modificare la logica matematica
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
        #endregion
        public class Posizione
        {
            public double Latitudine { get; set; }
            public double Longitudine { get; set; }
        }
    }
}
