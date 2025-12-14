using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inveni.App.Modelli
{
    public class IdUtenteRequest
    {
        public string? liveID { get; set; }              // LiveID del cellulare
        public string? deviceID { get; set; }            // DeviceID del cellulare
        public string? nomeApp { get; set; }             // Nome della App(Free – Pro – Android - iOS)
        public bool utenteTrial { get; set; }            // Definisce se Utente Free(=true) o Pro(=false)
        public string? versionePRG { get; set; }         // Versione sw del cellulare
        public string? comune { get; set; }              // Descrizione del Comune in osservazione
        public string? terminaleTipo { get; set; }       // Tipo del cellulare
        public string? terminaleFirmware { get; set; }   // Versione firmware del cellulare
        public string? linguaSistema { get; set; }       // Lingua di sistema del cellulare
        public string? linguaSelezionata { get; set; }   // Lingua selezionata del cellulare
    }

    public class IdUtenteResponse
    {
        public int id { get; set; }
        [JsonProperty("ruolo")]
        public bool isAdministrator { get; set; }
    }



   
    public class InviaLogSchedeRequest
    {
        // <summary>
        // 1) click sulla scheda
        // </summary>
        public const string? OPERAZIONE_SCHEDA = "SCHEDA";

        // <summary>
        // 2) consultazione anteprima sulla lista
        // 3) consultazione anteprima sulla mappa
        // </summary>
        public const string? OPERAZIONE_SCHEDA_PREVIEW = "SCHEDA PREVIEW";

        // <summary>
        // 4) Ascolto Brano audio al click su scheda
        // </summary>
        public const string? OPERAZIONE_BRANO_AUDIO = "BRANO AUDIO";

        // <summary>
        // 5) Ascolto Brano audio al click sulla anteprima della mappa
        // </summary>
        public const string? OPERAZIONE_BRANO_AUDIO_PREVIEW = "BRANO AUDIO PREVIEW";

        // <summary>
        // 6) Ascolto Brano audio per avvio automatico (in visita)
        // </summary>
        //public const string? OPERAZIONE_BRANO_AUDIO_AUTO = "BRANO AUDIO AUTO";

        // <summary>
        // 7) Click su video della scheda
        // </summary>
        public const string? OPERAZIONE_VIDEO_SCHEDA_CLICK = "VIDEO SCHEDA CLICK";

        // <summary>
        // 8) Avvio effettivo del video
        // </summary>
        public const string? OPERAZIONE_VIDEO_SCHEDA_AVVIO = "VIDEO SCHEDA AVVIO";

        // <summary>
        // 9) consultazione Itinerario
        // </summary>
        public const string? OPERAZIONE_ITINERARIO = "ITINERARIO";

        // <summary>
        // 10) avvio automatico scheda
        // </summary>
        public const string? OPERAZIONE_BRANO_SCHEDA_AUTO = "BRANO SCHEDA AUTO";


        public int IdUtente { get; set; }               // IdUtente
        public double Latitudine { get; set; }          // Latitudine attuale
        public double Longitudine { get; set; }         // Longitudine attuale
        public double Precisione { get; set; }          // Precisione GPS attuale
        public string? Comune { get; set; }              // Comune visualizzato
        public int CodiceCategoria { get; set; }        // Categoria scheda (se disponibile)
        public int CodiceServizio { get; set; }         // Servizio scheda (se disponibile)
        public int CodiceScheda { get; set; }           // ID Scheda (se disponibile)
        public string? NomeFileMP3 { get; set; }         // Nome file MP3 in ascolto (se disponibile)
        public string? Operazione { get; set; }          // Operazione effettuata
    }

    public class InviaLogGiochiRequest
    {
        public const string? OPERAZIONE_AVVIO_CACCIA_AL_TESORO = "AVVIO CACCIA AL TESORO";
        public const string? OPERAZIONE_USCITA_CACCIA_AL_TESORO = "USCITA CACCIA AL TESORO";
        public const string? OPERAZIONE_ENTRATA_AREA_INDIZIO = "ENTRATA AREA INDIZIO";
        public const string? OPERAZIONE_USCITA_AREA_INDIZIO = "USCITA AREA INDIZIO";
        public const string? OPERAZIONE_ENTRATA_AREA_ENIGMA = "ENTRATA AREA ENIGMA";
        public const string? OPERAZIONE_USCITA_AREA_ENIGMA = "USCITA AREA ENIGMA";
        public const string? OPERAZIONE_ENTRATA_AREA_CACCIA = "ENTRATA AREA CACCIA";
        public const string? OPERAZIONE_USCITA_AREA_CACCIA = "USCITA AREA CACCIA";
        public const string? OPERAZIONE_INVIO_PRIMA_NOTIFICA_ENIGMA = "INVIO PRIMA NOTIFICA ENIGMA";
        public const string? OPERAZIONE_INVIO_ALTRA_NOTIFICA_ENIGMA = "INVIO ALTRA NOTIFICA ENIGMA";
        public const string? OPERAZIONE_TROVATO_TESORO  = "TROVATO TESORO";
        public const string? OPERAZIONE_FINE_CACCIA_AL_TESORO = "FINE CACCIA AL TESORO";
        public const string? OPERAZIONE_RESET_TROVATO_TESORO = "RESET TROVATO TESORO";
        public const string? OPERAZIONE_FALLITO_TESORO = "FALLITO TESORO";



        public int IdUtente { get; set; }               // IdUtente
        public double Latitudine { get; set; }          // Latitudine attuale
        public double Longitudine { get; set; }         // Longitudine attuale
        //public double Precisione { get; set; }        // Precisione GPS attuale
        public string? Comune { get; set; }              // Comune visualizzato
        public int CodiceCategoria { get; set; }        // Categoria scheda (se disponibile)
        public int CodiceServizio { get; set; }         // Servizio scheda (se disponibile)
        public int CodiceScheda { get; set; }           // ID Scheda (se disponibile)
        public string? NomeFileMP3 { get; set; }         // Nome file MP3 in ascolto (se disponibile)
        public string? Operazione { get; set; }          // Operazione effettuata
        public int IdGioco { get; set; }                // IdGioco
    }

    public class InviaLogSchedeResponse
    {
        public string? messaggio { get; set; }
    }


    public class InviaLogGiochiResponse
    {
        public string? messaggio { get; set; }
    }
}
