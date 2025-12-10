using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmipedo.Models
{
    public class LeggiIdUtenteResponse
    {
        /// <summary>
        /// LiveID del cellulare
        /// </summary>
        public string liveID { get; set; }

        /// <summary>
        /// DeviceID del cellulare
        /// </summary>
        public string deviceID { get; set; }

        /// <summary>
        /// Nome della App(Free – Pro – Android - iOS)
        /// </summary>
        public string nomeApp { get; set; }

        /// <summary>
        /// Definisce se Utente Free(=true) o Pro(=false)
        /// </summary>
        public bool utenteTrial { get; set; }

        /// <summary>
        /// Versione sw del cellulare
        /// </summary>
        public string versioneSW { get; set; }

        /// <summary>
        /// Descrizione del Comune in osservazione
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Tipo del cellulare
        /// </summary>
        public string terminaleTipo { get; set; }

        /// <summary>
        /// Versione firmware del cellulare
        /// </summary>
        public string terminaleFirmware { get; set; }

        /// <summary>
        /// Lingua di sistema del cellulare
        /// </summary>
        public string linguaSistema { get; set; }

        /// <summary>
        /// Lingua selezionata del cellulare
        /// </summary>
        public string linguaSelezionata { get; set; }

        /// <summary>
        /// Ultima Data Ora memorizzata della sessione precedente
        /// </summary>
        public DateTime? lastDataOra { get; set; }

        /// <summary>
        /// Ultima Latitudine memorizzata della sessione precedente
        /// </summary>
        public decimal? lastLatitudine { get; set; }

        /// <summary>
        /// Ultima Longitudine memorizzata della sessione precedente
        /// </summary>
        public decimal? lastLongitudine { get; set; }

        /// <summary>
        /// Url per le notifiche
        /// </summary>
        public string urlMsgToast { get; set; }

        /// <summary>
        /// Email Utente(se registrato)
        /// </summary>
        public string webEmail { get; set; }
    }
}
