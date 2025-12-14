using System;
using System.Collections.Generic;
using System.Text;

namespace Inveni.App.Modelli
{
    public class ItemItinerario
    {
        public int itinerarioId { get; set; }
        public int _id { get; set; }
        public DateTime? DataCaccia { get; set; }
        public bool EsitoCaccia { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public int catId { get; set; }
        public string? name { get; set; }
        public int serviceId { get; set; }
        public int schedaCode { get; set; }
        public int ndistanza { get; set; }
        public TimeSpan hdurata { get; set; }
        public int IdGioco { get; set; }
        public int IdUtente { get; set; }
        public int? idGruppo { get; set; }
        public int? TentativiCaccia { get; set; }
        public int? TentativiEffettuati { get; set; }
        public int? PrecisioneCaccia { get; set; }
        public double? InclinazioneDa { get; set; }
        public double? InclinazioneA { get; set; }

        public string? Photo { get; set; }
        public string? comune { get; set; }
    }
}
