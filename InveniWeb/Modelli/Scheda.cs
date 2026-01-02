using System;

namespace InveniWeb.Modelli
{
    public class Scheda
    {
        // IDENTIFICAZIONE E ORGANIZZAZIONE
        public int Id { get; set; }
        public int IdOrganizzatore { get; set; }

        // CATEGORIZZAZIONE
        public int Categoria { get; set; } = 1;
        public int SubCategoria { get; set; } = 1;
        public int TipoScheda { get; set; }  // 1=TR1, 2=TR2, 3=TR3, 4=TR4, 5=TR5
        public int SequenzaTesori { get; set; } // Progressivo del Tesoro

        // CONTENUTO
        public string Titolo { get; set; } = string.Empty;
        public string? Descrizione { get; set; }

        // GEOLOCALIZZAZIONE
        public int? Raggio { get; set; }
        public string? Comune { get; set; }
        public string? Localita { get; set; }
        public double? Latitudine { get; set; }
        public double? Longitudine { get; set; }

        // TEMPORALI
        public DateTime DataCreazione { get; set; } = DateTime.Now;
        public DateTime UltimaModifica { get; set; } = DateTime.Now;

        // STATO
        public int Stato { get; set; } // 0=Bozza, 1=Pubblicata, 2=Archiviata

        // CAMPI PER TR1 (Descrizione Caccia)
        public DateTime? DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
        public string? PremioDescrizione { get; set; }

        // CAMPI PER TR2/TR3 (Area Attenzione - Enigma/Indizio)
        public string? TestoEnigma { get; set; }
        public string? AudioEntrataArea { get; set; }

        // CAMPI PER TR1 (Metriche Caccia)
        public int? LunghezzaCaccia { get; set; }
        public int? NumTappe { get; set; }

        // CAMPI PER TR4/TR5 (Area Caccia)
        public int? TentativiCaccia { get; set; }
        public int? TolleranzaBussola { get; set; }
        public decimal? Inclinazione { get; set; }
        public decimal? TolleranzaInclinazione { get; set; }

        // METODI UTILITY
        public bool IsCaccia() => TipoScheda == 1;
        public bool IsAreaAttenzione() => TipoScheda == 2 || TipoScheda == 3;
        public bool IsAreaCaccia() => TipoScheda == 4 || TipoScheda == 5;
        public bool IsEnigma() => TipoScheda == 3 || TipoScheda == 5;
        public bool IsIndizio() => TipoScheda == 2 || TipoScheda == 4;
    }
}