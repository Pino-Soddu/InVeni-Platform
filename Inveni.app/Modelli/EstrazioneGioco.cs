using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inveni.App.Modelli
{
    public class Gioco
    {
        public int IdUtente { get; set; }               // id Utente (Serve per ricostruire il gioco dell'Utente)
        public int IdGioco { get; set; }                // id Gioco
        public int _id { get; set; }                    // Progressivo Tappa
        public int? idGruppo { get; set; }              // Progressivo Gruppo (identifica e associa tutte le schede di una caccia)
        public double lat { get; set; }                 // Latitudine Nodo (WGS84)
        public double lon { get; set; }                 // Longitudine Nodo  (WGS84)
        public int catId { get; set; }                  // Codice Categoria (opzionale)
        public int serviceId { get; set; }              // Codice Servizio (opzionale)
        public int schedaCode { get; set; }             // Codice Scheda (opzionale)
        public string? name { get; set; }                // Descrizione Scheda -- lang --
        public string? DataCaccia { get; set; }          // Riporta l'eventuale data di risoluzione della tappa (vuoto=non risolto)
        public bool EsitoCaccia { get; set; }           // Riporta lo stato di risoluzione della tappa (0=non risolto, 1=risolto)
        public int? ndistanza { get; set; }             // Distanza dal nodo precedente (o Tappa precedente)
        public string? hdurata { get; set; }             // Durata percorso dal nodo precedente hh:mm:ss (o Tappa precedente)
        public int? PrecisioneCaccia { get; set; }      // Precisione dell'algoritmo di verifica della Caccia
        public int? TentativiCaccia { get; set; }       // Numero dei tentativi di una singola caccia
        public double? InclinazioneDa { get; set; }     // Angolo iniziale (da-a) dell'inclinazione Y dell'accelerometro
        public double? InclinazioneA { get; set; }      // Angolo finale (da-a) dell'inclinazione Y dell'accelerometro
        public string? AreaEnigmaMp3 { get; set; }       // Musica di sottofondo all'entrata dell'area Enigma
        public string? Algoritmo { get; set; }           // Se Tesoro trovato, contiene il risultato dell'Algoritmo
        public string? Bussola { get; set; }             // Se Tesoro trovato, contiene il valore della Bussola
        public string? InclinazioneX { get; set; }       // Se Tesoro trovato, contiene il valore dell'asse X dell'Accelerometro
        public int? TentativiEffettuati { get; set; }   // Numero dei tentativi effettuati per ogni caccia
        public string? DataUltimoAccesso { get; set; }   // data ultimo Accesso/Evento

        // Campi multimediali Scheda
        public string? video { get; set; }      // Da Schede.Film
        public string? audio { get; set; }      // Da Schede.Parlato  
        public string? text { get; set; }       // Da Schede.Testo
        public string? photo1 { get; set; }     // Da Multimedia.Foto

        public string? comune { get; set; }     // Da Schede.Comune
        public string? localitaCaccia { get; set; }  // Da Schede.Dati04
        public string? lunghezzaCaccia { get; set; } // Da Schede.Dati05
        public string? numTappeCaccia { get; set; }  // Da Schede.Dati06

        // Campi data per filtro/stato
        public DateTime? dataInizio { get; set; }
        public DateTime? dataFine { get; set; }
    }
}

