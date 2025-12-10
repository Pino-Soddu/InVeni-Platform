using System;
using System.Collections.Generic;
using System.Text;

namespace Palmipedo.Models
{
    public class Itinerario
    {
        public DateTime DataBackend { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataUltimoAgg { get; set; }
        public string LuogoItinerario { get; set; }
        public int _id { get; set; }
        public string autore { get; set; }
        public string nazione { get; set; }
        public string comune { get; set; }
        public int lunghezza { get; set; }
        public int nDurata { get; set; }
        public string name { get; set; }
        public TimeSpan hDurata { get; set; }
        public int nsiti { get; set; }
        public string image { get; set; }
        public string tipo { get; set; }
        public string indirizzopartenza { get; set; }
        public string indirizzoarrivo { get; set; }
        public string velMedia { get; set; }
    }
}
