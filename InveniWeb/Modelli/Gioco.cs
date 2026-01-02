#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InveniWeb.Modelli
{
    public class Gioco
    {
        public int IdUtente { get; set; }               // id Utente (Serve per ricostruire il gioco dell'Utente)
        
        [JsonPropertyName("idGioco")]
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
        // NUOVI CAMPI AGGIUNTI
        public string? premio { get; set; }      // Descrizione del premio
        public string? comesigioca { get; set; } // Regole del gioco

        public string? comune { get; set; }     // Da Schede.Comune
        public string? organizzatore { get; set; }  // Da Schede.Organizzatore
        public bool? topCaccia { get; set; }  // Da Schede.TOP:SK
        public string? localitaCaccia { get; set; }  // Da Schede.Dati04
        public string? lunghezzaCaccia { get; set; } // Da Schede.Dati05
        public string? numTappeCaccia { get; set; }  // Da Schede.Dati06

        // Campi data per filtro/stato
        public DateTime? dataInizio { get; set; }
        public DateTime? dataFine { get; set; }

        public string? PercorsoFotoCompleto
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

        public bool HasFotoSemplice => !string.IsNullOrEmpty(photo1);

        /// <summary>
        /// Testo formattato per il numero di tappe
        /// </summary>
        public string TestoTappe
        {
            get
            {
                if (string.IsNullOrEmpty(numTappeCaccia))
                    return "🚩 ? tappe";

                if (int.TryParse(numTappeCaccia, out int tappe))
                    return $"🚩 {tappe} tappe";

                return $"🚩 {numTappeCaccia}";
            }
        }

        /// <summary>
        /// Testo formattato per la lunghezza
        /// </summary>
        public string TestoLunghezza
        {
            get
            {
                if (string.IsNullOrEmpty(lunghezzaCaccia))
                    return "📏 ? km";

                if (int.TryParse(lunghezzaCaccia, out int metri))
                {
                    if (metri >= 1000)
                    {
                        double km = metri / 1000.0;
                        return $"📏 {km:F1} km";
                    }
                    else
                    {
                        return $"📏 {metri} m";
                    }
                }

                return $"📏 {lunghezzaCaccia}";
            }
        }

        /// <summary>
        /// Periodo completo di validità (sempre entrambe le date)
        /// </summary>
        public string TestoPeriodoCompleto
        {
            get
            {
                if (dataInizio == null || dataFine == null)
                    return "🗓️ Periodo non specificato";

                return $"🗓️ {dataInizio.Value:dd/MM/yy} - {dataFine.Value:dd/MM/yy}";
            }
        }

        /// <summary>
        /// Testo formattato per organizzatore (con icona)
        /// </summary>
        public string TestoOrganizzatore
        {
            get
            {
                if (string.IsNullOrEmpty(organizzatore))
                    return "👥 Non specificato";

                return $"👥 {organizzatore}";
            }
        }

        /// <summary>
        /// Testo formattato per comune (con icona)
        /// </summary>
        public string TestoComune
        {
            get
            {
                if (string.IsNullOrEmpty(comune))
                    return "🗺️ Non specificato";

                return $"🗺️ {comune}";
            }
        }

        // Per lunghezza senza icona
        public string TestoLunghezzaSenzaIcona
        {
            get
            {
                if (string.IsNullOrEmpty(lunghezzaCaccia))
                    return "? km";

                if (int.TryParse(lunghezzaCaccia, out int metri))
                {
                    if (metri >= 1000)
                    {
                        double km = metri / 1000.0;
                        return $"{km:F1} km";
                    }
                    else
                    {
                        return $"{metri} m";
                    }
                }
                return lunghezzaCaccia;
            }
        }

        // Per periodo senza icona (formato compatto)
        public string TestoPeriodoSenzaIcona
        {
            get
            {
                if (dataInizio == null || dataFine == null)
                    return "N/D";

                // Formato compatto: "01/01-01/12" o "01/01/25-01/12/26"
                return $"{dataInizio.Value:dd/MM}-{dataFine.Value:dd/MM}";
            }
        }

        // Per periodo CON anno ma compatto
        public string TestoPeriodoConAnno
        {
            get
            {
                if (dataInizio == null || dataFine == null)
                    return "N/D";

                // Se stesso anno: "01/01-01/12/25"
                // Anni diversi: "01/01/25-01/12/26"
                if (dataInizio.Value.Year == dataFine.Value.Year)
                {
                    return $"{dataInizio.Value:dd/MM}-{dataFine.Value:dd/MM}/{dataInizio.Value:yy}";
                }
                else
                {
                    return $"{dataInizio.Value:dd/MM/yy}-{dataFine.Value:dd/MM/yy}";
                }
            }
        }

        // Per periodo con mesi abbreviati
        public string TestoPeriodoMesi
        {
            get
            {
                if (dataInizio == null || dataFine == null)
                    return "N/D";

                var inizio = dataInizio.Value;
                var fine = dataFine.Value;

                // Abbreviazioni mesi italiane
                var mesi = new[] { "gen", "feb", "mar", "apr", "mag", "giu",
                          "lug", "ago", "set", "ott", "nov", "dic" };

                string meseInizio = mesi[inizio.Month - 1];
                string meseFine = mesi[fine.Month - 1];

                // Se stesso mese: "01-15 gen"
                if (inizio.Month == fine.Month && inizio.Year == fine.Year)
                {
                    return $"{inizio:dd}-{fine:dd} {meseInizio}";
                }
                // Se stesso anno: "01 gen - 15 mar"
                else if (inizio.Year == fine.Year)
                {
                    return $"{inizio:dd} {meseInizio} - {fine:dd} {meseFine}";
                }
                // Anni diversi: "01 gen 25 - 15 mar 26"
                else
                {
                    return $"{inizio:dd} {meseInizio} {inizio:yy} - {fine:dd} {meseFine} {fine:yy}";
                }
            }
        }

        // PROPRIETÀ CALCOLATE PER IL PREMIO (opzionale, ma utile)
        public bool HaPremio => !string.IsNullOrEmpty(premio);
        public bool HaRegole => !string.IsNullOrEmpty(comesigioca);
    }
}

