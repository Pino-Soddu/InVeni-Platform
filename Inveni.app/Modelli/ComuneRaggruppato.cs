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
    }
}