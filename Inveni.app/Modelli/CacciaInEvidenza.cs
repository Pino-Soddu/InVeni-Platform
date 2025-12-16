using System;

namespace Inveni.App.Modelli
{
    /// <summary>
    /// Wrapper per cacce in evidenza con proprietà di visualizzazione aggiuntive
    /// </summary>
    public class CacciaInEvidenza
    {
        public Gioco Caccia { get; set; }

        /// <summary>
        /// Indica se la caccia è attiva (true) o programmata (false)
        /// </summary>
        public bool IsAttiva
        {
            get
            {
                if (Caccia.dataInizio == null || Caccia.dataFine == null)
                    return false;

                var now = DateTime.Now;
                return Caccia.dataInizio <= now && Caccia.dataFine >= now;
            }
        }

        /// <summary>
        /// Colore di sfondo in base allo stato (attiva/programmata)
        /// </summary>
        public string ColoreSfondo
        {
            get
            {
                return IsAttiva ? "#E8F5E9" : "#FFF3E0"; // Verde chiaro / Arancione chiaro
            }
        }

        /// <summary>
        /// Colore bordo in base allo stato
        /// </summary>
        public string ColoreBordo
        {
            get
            {
                return IsAttiva ? "#4CAF50" : "#FF9800"; // Verde / Arancione
            }
        }

        /// <summary>
        /// Testo stato per visualizzazione
        /// </summary>
        public string TestoStato
        {
            get
            {
                return IsAttiva ? "ATTIVA ORA" : "IN PROGRAMMA";
            }
        }

        public CacciaInEvidenza(Gioco caccia)
        {
            Caccia = caccia ?? throw new ArgumentNullException(nameof(caccia));
        }
    }
}