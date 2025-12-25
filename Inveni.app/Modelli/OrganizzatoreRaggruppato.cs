using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace Inveni.App.Modelli
{
    /// <summary>
    /// Modello per organizzare le cacce per organizzatore
    /// </summary>
    public class OrganizzatoreRaggruppato
    {
        public string NomeOrganizzatore { get; set; } = string.Empty;
        public int TotaleCacce { get; set; }
        public int CacceAttive { get; set; }
        public int CacceProgrammate { get; set; }
        public int CacceScaduteDisponibili { get; set; }
        public List<Gioco> CacceDettaglio { get; set; } = new();

        /// <summary>
        /// Costruttore per inizializzare con il nome dell'organizzatore
        /// </summary>
        public OrganizzatoreRaggruppato(string nomeOrganizzatore)
        {
            NomeOrganizzatore = nomeOrganizzatore;
        }

        public string ImmagineOrganizzatore
        {
            get
            {
                //Console.WriteLine($"★★★ GETTER DETTAGLIO ★★★");
                //Console.WriteLine($"★★★ NomeOrganizzatore: '{NomeOrganizzatore}'");

                if (string.IsNullOrEmpty(NomeOrganizzatore))
                {
                    Console.WriteLine($"⚠️⚠️⚠️ NomeOrganizzatore è VUOTO!");
                    return "org_default.jpg";
                }

                // 1. Converti in lowercase
                var nomeFile = NomeOrganizzatore.ToLower();

                // 2. Rimuovi accenti (gestione completa italiano)
                nomeFile = RimuoviAccenti(nomeFile);

                // 3. Sostituisci caratteri speciali con underscore
                nomeFile = Regex.Replace(nomeFile, @"[^a-z0-9]", "_");

                // 4. Rimuovi underscore multipli consecutivi
                nomeFile = Regex.Replace(nomeFile, @"_+", "_");

                // 5. Rimuovi underscore iniziali/finali
                nomeFile = nomeFile.Trim('_');

                return $"org_{nomeFile}.jpg";
            }
        }

        // ★★★ METODO PER RIMUOVERE ACCENTI ★★★
        private string RimuoviAccenti(string testo)
        {
            if (string.IsNullOrWhiteSpace(testo))
                return testo;

            // Tabella di conversione caratteri accentati italiani
            var accenti = new Dictionary<char, char>
    {
        {'à', 'a'}, {'è', 'e'}, {'é', 'e'}, {'ì', 'i'}, {'ò', 'o'}, {'ù', 'u'},
        {'À', 'A'}, {'È', 'E'}, {'É', 'E'}, {'Ì', 'I'}, {'Ò', 'O'}, {'Ù', 'U'},
        {'á', 'a'}, {'í', 'i'}, {'ó', 'o'}, {'ú', 'u'},
        {'Á', 'A'}, {'Í', 'I'}, {'Ó', 'O'}, {'Ú', 'U'},
        {'â', 'a'}, {'ê', 'e'}, {'î', 'i'}, {'ô', 'o'}, {'û', 'u'},
        {'Â', 'A'}, {'Ê', 'E'}, {'Î', 'I'}, {'Ô', 'O'}, {'Û', 'U'},
        {'ä', 'a'}, {'ë', 'e'}, {'ï', 'i'}, {'ö', 'o'}, {'ü', 'u'},
        {'Ä', 'A'}, {'Ë', 'E'}, {'Ï', 'I'}, {'Ö', 'O'}, {'Ü', 'U'}
    };

            var risultato = new char[testo.Length];

            for (int i = 0; i < testo.Length; i++)
            {
                if (accenti.TryGetValue(testo[i], out char sostituto))
                    risultato[i] = sostituto;
                else
                    risultato[i] = testo[i];
            }

            return new string(risultato);
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