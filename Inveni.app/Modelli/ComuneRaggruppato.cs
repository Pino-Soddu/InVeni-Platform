using System.Collections.Generic;

namespace Inveni.App.Modelli
{
    public class ComuneRaggruppato
    {
        public string NomeComune { get; set; } = string.Empty;
        public int TotaleCacce { get; set; }
        public int CacceAttive { get; set; }
        public int CacceProgrammate { get; set; }
        public int CacceScaduteDisponibili { get; set; }
        public List<Gioco> CacceDettaglio { get; set; } = new();

        // Costruttore per facilitare la creazione
        public ComuneRaggruppato(string nomeComune)
        {
            NomeComune = nomeComune;
        }

        public string ImmagineComune
        {
            get
            {
                // Costruisci: "comune_" + nomedelcomune (senza spazi, lowercase)
                var nomeFile = NomeComune
                    .ToLower()
                    .Replace(" ", "")
                    .Replace("'", "");

                return $"comune_{nomeFile}.png";
            }
        }

        public Color ColoreBordoStato
        {
            get
            {
                if (CacceAttive > 0) return Color.FromArgb("#4CAF50");    // Verde per attive
                if (CacceProgrammate > 0) return Color.FromArgb("#FF9800"); // Arancione per programmate
                return Color.FromArgb("#666666");                         // Grigio per scadute
            }
        }
    }
}