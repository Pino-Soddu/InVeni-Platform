using System.Collections.Generic;

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
    }
}