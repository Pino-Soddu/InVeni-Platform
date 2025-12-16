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
        public string? organizzatore { get; set; }  // Da Schede.Organizzatore
        public bool? topCaccia { get; set; }  // Da Schede.TOP:SK
        public string? localitaCaccia { get; set; }  // Da Schede.Dati04
        public string? lunghezzaCaccia { get; set; } // Da Schede.Dati05
        public string? numTappeCaccia { get; set; }  // Da Schede.Dati06

        // Campi data per filtro/stato
        public DateTime? dataInizio { get; set; }
        public DateTime? dataFine { get; set; }

        public string PercorsoFotoCompleto
        {
            get
            {
                // SEMPLICE: Costruisci il percorso e basta
                // Non controllare se il file esiste
                // Il backend sa quello che fa

                if (string.IsNullOrEmpty(comune) || string.IsNullOrEmpty(photo1))
                    return null;

                return Path.Combine(
                    Costanti.PercorsiLocali.BaseArchivio,
                    comune,
                    Costanti.PercorsiLocali.CartellaFoto,
                    photo1
                );
            }
        }

        /// <summary>
        /// Colore del bordo in base allo stato della caccia
        /// </summary>
        public Color ColoreBordoStato
        {
            get
            {
                if (dataInizio == null || dataFine == null)
                    return Costanti.ColoriStato.Storico;

                var now = DateTime.Now;

                if (dataInizio <= now && dataFine >= now)
                    return Costanti.ColoriStato.GiocaOra;      // Attiva
                else if (dataInizio > now)
                    return Costanti.ColoriStato.InProgramma;   // Programmata
                else
                    return Costanti.ColoriStato.Storico;       // Storica
            }
        }

        /// <summary>
        /// Indica se la caccia è TOP (in evidenza)
        /// </summary>
        public bool IsTopCaccia => topCaccia == true;

        /// <summary>
        /// Testo del contatore stato (es: "3 cacce")
        /// </summary>
        public string ContatoreTesto(int conteggio)
        {
            return conteggio == 1 ? "1 caccia" : $"{conteggio} cacce";
        }

        /// <summary>
        /// Ottiene l'iniziale del comune (per fallback)
        /// </summary>
        public string InizialeComune
        {
            get
            {
                if (string.IsNullOrEmpty(comune))
                    return "?";

                return comune.Trim().Substring(0, 1).ToUpper();
            }
        }

        /// <summary>
        /// Colore per il fallback (se non c'è foto)
        /// Basato sul nome del comune per consistenza
        /// </summary>
        public Color ColoreFallbackComune
        {
            get
            {
                if (string.IsNullOrEmpty(comune))
                    return Colors.Gray;

                // Genera colore consistente dal nome del comune
                var nome = comune.ToUpper();
                var hash = nome.GetHashCode();

                var coloriDisponibili = new[]
                {
                    Color.FromArgb("#2196F3"), // Blu
                    Color.FromArgb("#4CAF50"), // Verde
                    Color.FromArgb("#FF9800"), // Arancione
                    Color.FromArgb("#9C27B0"), // Viola
                    Color.FromArgb("#F44336"), // Rosso
                    Color.FromArgb("#00BCD4"), // Ciano
                };

                var index = Math.Abs(hash) % coloriDisponibili.Length;
                return coloriDisponibili[index];
            }
        }

        /// <summary>
        /// Proprietà che restituisce direttamente ImageSource
        /// </summary>
        public ImageSource FotoSource
        {
            get
            {
                // 1. Ottieni il percorso stringa
                var percorso = PercorsoFotoCompleto;

                // 2. Se non c'è percorso, ritorna null (fallback si attiva)
                if (string.IsNullOrEmpty(percorso))
                    return null;

                // 3. Verifica se il file esiste
                try
                {
                    if (System.IO.File.Exists(percorso))
                    {
                        // 4. Converti esplicitamente in ImageSource
                        return ImageSource.FromFile(percorso);
                    }
                }
                catch
                {
                    // Ignora errori, fallback si attiverà
                }

                return null;
            }
        }
    }
}

