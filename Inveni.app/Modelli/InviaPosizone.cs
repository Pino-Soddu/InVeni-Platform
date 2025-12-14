using System;
using System.Collections.Generic;
using System.Text;

namespace Inveni.App.Modelli
{
    public class InviaPosizioneResponse
    {
        public string? messaggio { get; set; }
    }

    public class InviaPosizioneRequest
    {
        public int IdUtente { get; set; }               // IdUtente
        public int Visible { get; set; }                // Stato Visibile (0=no, 1=sì)
        public double Latitudine { get; set; }          // Latitudine attuale
        public double Longitudine { get; set; }         // Longitudine attuale
        public double Precisione { get; set; }          // Precisione GPS attuale
        public double LatAgg { get; set; }              // Latitudine agganciata (solo per Windows)
        public double LonAgg { get; set; }              // Longitudine agganciata (solo per Windows)
        public int IdGioco { get; set; }
    }
}
