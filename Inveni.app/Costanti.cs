using System;

namespace Inveni.App
{
    public static class Costanti
    {
        // PERCORSI ARCHIVIO LOCALE (SVILUPPO)
        public static class PercorsiLocali
        {
            // PERCORSO PUBBLICO CHE HAI CREATO
            //public static string BaseArchivio = @"C:\Users\Public\Pictures\InveniFoto\";
            public static string BaseArchivio = @"file:///C:/Users/Public/Pictures/InveniFoto/";
            public static string CartellaFoto = "Foto";

            public static string PercorsoFotoCompleto(string comune, string nomeFile)
            {
                if (string.IsNullOrEmpty(comune) || string.IsNullOrEmpty(nomeFile))
                    return null;

                var percorso = Path.Combine(
                    BaseArchivio,
                    comune.Trim(),
                    CartellaFoto,
                    nomeFile
                );

                // DEBUG: STAMPA PER VERIFICA
                Console.WriteLine($"📍 Percorso foto: {percorso}");

                return percorso;
            }
        }

        // COLORI PER STATO CACCE
        public static class ColoriStato
        {
            public static readonly Color GiocaOra = Color.FromArgb("#4CAF50");    // Verde
            public static readonly Color InProgramma = Color.FromArgb("#FF9800"); // Arancione
            public static readonly Color Storico = Color.FromArgb("#9E9E9E");     // Grigio
        }

        // ALTRE COSTANTI
        public static readonly Color ColoreTopBadge = Color.FromArgb("#FFD700"); // Oro
    }
}