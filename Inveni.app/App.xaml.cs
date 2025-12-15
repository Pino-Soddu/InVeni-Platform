using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inveni.App
{
    public partial class App : Application
    {
        // POSIZIONE GPS SIMULATA (per sviluppo)
        // Posizione fissa: Roma (Colosseo)
        public static class PosizioneSimulata
        {
            public static double Latitudine { get; } = 41.8902;
            public static double Longitudine { get; } = 12.4922;
            public static string Citta { get; } = "ROMA";

            // Metodo per calcolare distanza da un punto (formula semplificata)
            public static double CalcolaDistanzaKm(double latDest, double lonDest)
            {
                // Formula di Haversine semplificata per sviluppo
                var R = 6371; // Raggio Terra in km
                var dLat = ToRadians(latDest - Latitudine);
                var dLon = ToRadians(lonDest - Longitudine);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(ToRadians(Latitudine)) * Math.Cos(ToRadians(latDest)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return Math.Round(R * c, 2); // Distanza in km, arrotondata a 2 decimali
            }

            private static double ToRadians(double angle)
            {
                return Math.PI * angle / 180.0;
            }
        }

        // COLLEZIONE CACCE CONDIVISA (semplice - si può migliorare)
        private static List<Models.Caccia> _cacceGlobali = new();
        public static List<Models.Caccia> CacceGlobali
        {
            get => _cacceGlobali;
            set => _cacceGlobali = value ?? new List<Models.Caccia>();
        }

        // STATO APPLICAZIONE
        public static bool IsInitialized { get; set; } = false;

        public App()
        {
            InitializeComponent();

            // Imposta MainPage come AppShell
            MainPage = new AppShell();

            // Carica dati iniziali (mock per ora)
            CaricaDatiMock();
        }

        private void CaricaDatiMock()
        {
            // Dati di esempio - sostituire con chiamata API reale
            CacceGlobali = new List<Models.Caccia>
            {
                new Models.Caccia
                {
                    IdGioco = 180269,
                    Name = "Caccia a Antium",
                    Organizzatore = "PROLOCO DI ANZIO",
                    Comune = "ANZIO",
                    Photo1 = "https://via.placeholder.com/150",
                    Stato = "attiva",
                    DistanzaKm = PosizioneSimulata.CalcolaDistanzaKm(41.4491, 12.6214), // Anzio
                    LocalitaCaccia = "Centro storico",
                    LunghezzaCaccia = "2,5 km",
                    NumTappeCaccia = "8"
                },
                new Models.Caccia
                {
                    IdGioco = 180270,
                    Name = "Tesori di Ostia",
                    Organizzatore = "COMUNE DI ROMA",
                    Comune = "ROMA",
                    Photo1 = "https://via.placeholder.com/150",
                    Stato = "attiva",
                    DistanzaKm = PosizioneSimulata.CalcolaDistanzaKm(41.7592, 12.3000), // Ostia
                    LocalitaCaccia = "Scavi di Ostia Antica",
                    LunghezzaCaccia = "3,2 km",
                    NumTappeCaccia = "10"
                },
                new Models.Caccia
                {
                    IdGioco = 180271,
                    Name = "Misteri del Colosseo",
                    Organizzatore = "SOPRINTENDENZA ARCHEOLOGIA",
                    Comune = "ROMA",
                    Photo1 = "https://via.placeholder.com/150",
                    Stato = "programmata",
                    DistanzaKm = 0.5, // Vicino al Colosseo
                    LocalitaCaccia = "Foro Romano",
                    LunghezzaCaccia = "1,8 km",
                    NumTappeCaccia = "6"
                }
            };

            IsInitialized = true;
        }

        // Metodo per ricaricare dati da API (da chiamare quando necessario)
        public static async Task RicaricaDatiDaApi()
        {
            // TODO: Implementare chiamata reale a ApiServizio
            await Task.Delay(100); // Simula caricamento
        }
    }
}