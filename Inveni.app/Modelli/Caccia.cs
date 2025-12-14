using System;
using System.Collections.Generic;
using System.Text;

namespace Inveni.App.Modelli
{
    public class EstrazioneTracciamento
    {
        public int IdUtente { get; set; }               // id Utente 
        public int IdGioco { get; set; }                // id Gioco
        public List<Punto> Punti { get; set; } = new List<Punto>();
    }

    public class Punto
    {
        public double lat { get; set; }                 // Latitudine Nodo (WGS84)
        public double lon { get; set; }                 // Longitudine Nodo  (WGS84)
    }
}
