using System;
using System.Collections.Generic;
using System.Text;

namespace Palmipedo.Models
{
    public class Langs
    {
        public const string LANG_ITA = "ITA";
        public const string LANG_ENG = "ENG";
        public const string LANG_SPA = "SPA";
        public const string LANG_FRA = "FRA";
        public const string LANG_DEU = "DEU";
        public const string LANG_RUS = "RUS";
    }
}


//exports.listaServizi = [
//  {
//    key: 'catId',	// Codice Categoria
//    type: 'categoryId'
//  },
//  {
//    key: 'serviceCode',	// Codice Servizio
//    type: 'serviceId'
//  },
//  {
//    key: 'name',	// Descrizione scheda ITA
//    lang: 'ITA',
//    type: 'capitalizeString'
//  },
//  '',			// Icona Utilizzata da palmipedo
//  '',			// Utilizzato da palmipedo #1
//  '',			// Utilizzato da palmipedo #2
//  '',			// Utilizzato da palmipedo #3
//  '',			// Utilizzato da palmipedo #4
//  '',			// Utilizzato da palmipedo #5
//  '',			// Utilizzato da palmipedo #6
//  '',			// Utilizzato da palmipedo #7
//  '',			// Utilizzato da palmipedo #8
//  '',			// Utilizzato da palmipedo #9
//  '',			// Utilizzato da palmipedo #10
//  '',			// Utilizzato da palmipedo #11
//  '',			// Utilizzato da palmipedo #12
//  '',			// Utilizzato da palmipedo #13
//  '',			// Utilizzato da palmipedo #14
//  '',			// Utilizzato da palmipedo #15
//  '',			// Utilizzato da palmipedo #16
//  '',			// Utilizzato da palmipedo #17
//  '',			// Utilizzato da palmipedo #18
//  '',			// Utilizzato da palmipedo #19
//  '',			// Utilizzato da palmipedo #20
//  '',			// Utilizzato da palmipedo #21
//  '',			// Utilizzato da palmipedo #22
//  '',			// Utilizzato da palmipedo #23
//  '',			// Utilizzato da palmipedo #24
//  '',			// Utilizzato da palmipedo #25
//  '',			// Utilizzato da palmipedo #26
//  '',			// Utilizzato da palmipedo #27
//  '',			// Utilizzato da palmipedo #28
//  '',			// Utilizzato da palmipedo #29
//  '',			// Utilizzato da palmipedo #30
//  '',			// Utilizzato da palmipedo #31
//  '',			// Utilizzato da palmipedo #32

//  {
//    key: 'name',	// Descrizione scheda ENG
//    lang: 'ENG',
//    type: 'capitalizeString'
//  },
//  {
//    key: 'name',	// Descrizione scheda SPA
//    lang: 'SPA',
//    type: 'capitalizeString'
//  },
//  {
//    key: 'name',	// Descrizione scheda DEU
//    lang: 'FRA',
//    type: 'capitalizeString',
//  },
//  {
//    key: 'name',	// Descrizione scheda FRA
//    lang: 'DEU',
//    type: 'capitalizeString'
//  },
//  {
//    key: 'name',	// Descrizione scheda RUS
//    lang: 'RUS'
//  },
//  '',			// Non utilizzato #1
//  '',			// Non utilizzato #2
//  '',			// Non utilizzato #3
//  '',			// Non utilizzato #4
//  'icoMenu',		// Icona del Servizio nei Menu
//  'icoMap',		// Icona del Servizio nella Mappa
//];

//exports.listaComuni = [
//  //
//  // Dati anagrafici
//  //
//  {
//    key: 'id',		// ID
//    type: 'int'
//  },
//  {
//    key: 'ProgSponsor',	// Progressivo (solo x Sponsor)
//    type: 'int'
//  },
//  {
//    key: 'AggrSponsor',	// Aggregazione (solo x Sponsor)
//    type: 'int'
//  },
//  'nazione', 		// Nazione
//  'name', 		// Descrizione Comune
//  'image', 		// Immagine Comune
//  'cityType',  		// Tipo città (vedi ws ListaTipiCitta)
//  'visible', 		// Comune Visibile (True/False)
//  'autostart ',		// Autostart (Utilizzato da Palmipedo)
//  {
//    key: 'catsInMap'	// Categorie/servizi da mappare configurate
//  },
//  {
//    key: 'catsDefault' // Categorie/servizi da mappare Accese e/o Default
//  },
//  {
//    key: 'langs' 	// Lingue Supportate
//  },
//  'nItinerari',		// Numero Itinerari
//  'nSchede',		// Totale schede
//  //
//  // Dati cartografici
//  //
//  'latC', 		// Latitudine Centrale (WGS84)
//  'lonC', 		// Longitudine Centrale  (WGS84)
//  'zoom', 		// Livello zoom Iniziale
//  {
//    key: 'userMap' 	// Mappa Utente (SI/NO)
//  },
//  'SO_Lat',		// Latitudine Vertice SO (WGS84)
//  'SO_Lng',		// Longitudine Vertice SO (WGS84)
//  'NE_Lat',		// Latitudine Vertice NE (WGS84)
//  'NE_Lng',		// Longitudine Vertice NE (WGS84)
//  '',			// Orientamento (Utilizzato da Palmipedo)
//  '',			// Profondità (Utilizzato da Palmipedo)
//  {
//    key: 'raggio',			// Raggio per dintorni (Utilizzato da Palmipedo)
//    type: 'int'
//  },
//  '',			// Aggregazione (Utilizzato da Palmipedo)
//  //
//  // Contenuti multimediali
//  //
//  {
//    key: 'nTexts', 	// Numero Testi (ITA)
//    lang: 'ITA'
//  },
//  {
//    key: 'nTexts', 	// Numero Testi (ENG)
//    lang: 'ENG'
//  },
//  {
//    key: 'nTexts', 	// Numero Testi (SPA)
//    lang: 'SPA'
//  },
//  {
//    key: 'nTexts', 	// Numero Testi (FRA)
//    lang: 'FRA'
//  },
//  {
//    key: 'nTexts', 	// Numero Testi (DEU)
//    lang: 'DEU'
//  },
//  {
//    key: 'nTexts', 	// Numero Testi (RUS)
//    lang: 'RUS'
//  },

//  {
//    key: 'nPhotos', 	// Numero Foto (ITA)
//    lang: 'ITA'
//  },
//  {
//    key: 'nPhotos', 	// Numero Foto (ENG)
//    lang: 'ENG'
//  },
//  {
//    key: 'nPhotos', 	// Numero Foto (SPA)
//    lang: 'SPA'
//  },
//  {
//    key: 'nPhotos', 	// Numero Foto (FRA)
//    lang: 'FRA'
//  },
//  {
//    key: 'nPhotos', 	// Numero Foto (DEU)
//    lang: 'DEU'
//  },
//  {
//    key: 'nPhotos', 	// Numero Foto (RUS)
//    lang: 'RUS'
//  },

//  {
//    key: 'nAudio', 	// Numero Brani Audio (ITA)
//    lang: 'ITA'
//  },
//  {
//    key: 'nAudio', 	// Numero Brani Audio (ENG)
//    lang: 'ENG'
//  },
//  {
//    key: 'nAudio', 	// Numero Brani Audio (SPA)
//    lang: 'SPA'
//  },
//  {
//    key: 'nAudio', 	// Numero Brani Audio (FRA)
//    lang: 'FRA'
//  },
//  {
//    key: 'nAudio', 	// Numero Brani Audio (DEU)
//    lang: 'DEU'
//  },
//  {
//    key: 'nAudio', 	// Numero Brani Audio (RUS)
//    lang: 'RUS'
//  },

//  {
//    key: 'lengthAudio', //  Durata Brani Audio (ITA)
//    lang: 'ITA'
//  },
//  {
//    key: 'lengthAudio', //  Durata Brani Audio (ENG)
//    lang: 'ENG'
//  },
//  {
//    key: 'lengthAudio', //  Durata Brani Audio (SPA)
//    lang: 'SPA'
//  },
//  {
//    key: 'lengthAudio', //  Durata Brani Audio (FRA)
//    lang: 'FRA'
//  },
//  {
//    key: 'lengthAudio', //  Durata Brani Audio (DEU)
//    lang: 'DEU'
//  },
//  {
//    key: 'lengthAudio', //  Durata Brani Audio (RUS)
//    lang: 'RUS'
//  },

//  {
//    key: 'mbTexts', 	//  Occupazione Testi (MB) (ITA)
//    lang: 'ITA'
//  },
//  {
//    key: 'mbTexts', 	//  Occupazione Testi (MB) (ENG)
//    lang: 'ENG'
//  },
//  {
//    key: 'mbTexts', 	//  Occupazione Testi (MB) (SPA)
//    lang: 'SPA'
//  },
//  {
//    key: 'mbTexts', 	//  Occupazione Testi (MB) (FRA)
//    lang: 'FRA'
//  },
//  {
//    key: 'mbTexts', 	//  Occupazione Testi (MB) (DEU)
//    lang: 'DEU'
//  },
//  {
//    key: 'mbTexts', 	//  Occupazione Testi (MB) (RUS)
//    lang: 'RUS'
//  },

//  {
//    key: 'mbPhotos', 	//  Occupazione Foto (MB) (ITA)
//    lang: 'ITA'
//  },
//  {
//    key: 'mbPhotos', 	//  Occupazione Foto (MB) (ENG)
//    lang: 'ENG'
//  },
//  {
//    key: 'mbPhotos', 	//  Occupazione Foto (MB) (SPA)
//    lang: 'SPA'
//  },
//  {
//    key: 'mbPhotos', 	//  Occupazione Foto (MB) (FRA)
//    lang: 'FRA'
//  },
//  {
//    key: 'mbPhotos', 	//  Occupazione Foto (MB) (DEU)
//    lang: 'DEU'
//  },
//  {
//    key: 'mbPhotos', 	//  Occupazione Foto (MB) (RUS)
//    lang: 'RUS'
//  },

//  {
//    key: 'mbAudio', 	//  Occupazione Audio (MB) (ITA)
//    lang: 'ITA'
//  },
//  {
//    key: 'mbAudio', 	//  Occupazione Audio (MB) (ENG)
//    lang: 'ENG'
//  },
//  {
//    key: 'mbAudio', 	//  Occupazione Audio (MB) (SPA)
//    lang: 'SPA'
//  },
//  {
//    key: 'mbAudio', 	//  Occupazione Audio (MB) (FRA)
//    lang: 'FRA'
//  },
//  {
//    key: 'mbAudio', 	//  Occupazione Audio (MB) (DEU)
//    lang: 'DEU'
//  },
//  {
//    key: 'mbAudio', 	//  Occupazione Audio (MB) (RUS)
//    lang: 'RUS'
//  }
//];

//exports.listaTipiCitta = [
//  {
//    key: 'code',
//    type: 'int'
//  },
//  {
//    key: 'order',
//    type: 'int'
//  },
//  {
//    key: 'name',
//    lang: 'ITA'
//  },
//  {
//    key: 'name',
//    lang: 'ENG'
//  },
//  {
//    key: 'name',
//    lang: 'SPA'
//  },
//  {
//    key: 'name',
//    lang: 'FRA'
//  },
//  {
//    key: 'name',
//    lang: 'DEU'
//  },
//  {
//    key: 'name',
//    lang: 'RUS'
//  }
//];

//exports.listaSchede = [
//  {
//    key: '_id',	// Id univoco Scheda
//    type: 'int'
//  },
//  {
//    key: 'userId',		// Codice Utente (0=Palmipedo, xx= Utenti)
//    type: 'int'
//  },
//  {
//    key: 'catId',	// Codice Categoria
//    type: 'categoryId'
//  },
//  {
//    key: 'serviceId',	// Codice Servizio
//    type: 'serviceId'
//  },
//  {
//    key: 'schedaCode',	// Codice Scheda
//    type: 'int'
//  },
//  {
//    key: 'name',	// Descrizione scheda ITA
//    lang: 'ITA',
//    type: 'capitalizeString'
//  },
//  {
//    key: 'name',	// Descrizione scheda ENG
//    lang: 'ENG'
//  },
//  {
//    key: 'name',	// Descrizione scheda SPA
//    lang: 'SPA'
//  },
//  {
//    key: 'name',	// Descrizione scheda DEU
//    lang: 'FRA'
//  },
//  {
//    key: 'name',	// Descrizione scheda FRA
//    lang: 'DEU'
//  },
//  {
//    key: 'name',	// Descrizione scheda RUS
//    lang: 'RUS'
//  },
//  '',			// Non utilizzato
//  '',			// Non utilizzato
//  '',			// Non utilizzato
//  '',			// Non utilizzato
//  'visible', 		// Visibilità Scheda(0=non visibile; 1= visibile)
//  {
//    key: 'radius',	// Raggio area di attraversamento
//    type: 'int'
//  },
//  {
//    key: 'priority',	// Top Scheda (1=TOP)
//    type: 'int'
//  },
//  'date',		// Data ultimo aggiornamento
//  'address',		// Indirizzo Completo
//  {
//    key: 'comune',
//    type: 'uppercaseString'
//  },		// descrizione Comune
//  'Telephone',		// Numero di Telefono
//  '',			// Utilizzato da palmipedo #1
//  '',			// Utilizzato da palmipedo #2
//  '',			// Utilizzato da palmipedo #3
//  '',			// Utilizzato da palmipedo #4
//  '',			// Utilizzato da palmipedo #5
//  '',			// Utilizzato da palmipedo #6
//  '',			// Utilizzato da palmipedo #7
//  '',			// Utilizzato da palmipedo #8
//  '',			// Utilizzato da palmipedo #9
//  '',			// Utilizzato da palmipedo #11
//  '',			// Utilizzato da palmipedo #11
//  '',			// Utilizzato da palmipedo #12
//  '',			// Utilizzato da palmipedo #13
//  '',			// Utilizzato da palmipedo #14
//  '',			// Utilizzato da palmipedo #15
//  '',			// Utilizzato da palmipedo #16
//  '',			// Utilizzato da palmipedo #17
//  {
//    key: 'nListens',	// Contatore ascolti Audioguida
//    type: 'int'
//  },
//  {
//    key: 'nVisits',	// Contatore visite scheda
//    type: 'int'
//  },
//  {
//    key: 'nClicks',	// Contatore click scheda
//    type: 'int'
//  },
//  '',			// Utilizzato da palmipedo #21
//  '',			// Utilizzato da palmipedo #22
//  '',			// Utilizzato da palmipedo #23
//  '',			// Utilizzato da palmipedo #24
//  '',			// Utilizzato da palmipedo #25
//  '',			// Utilizzato da palmipedo #26  {
//  {
//    key: 'lengthAudio', // Durata Audio ITA
//    lang: 'ITA'
//  },
//  {
//    key: 'lengthAudio', // Durata Audio ENG
//    lang: 'ENG'
//  },
//  {
//    key: 'lengthAudio', // Durata Audio SPA
//    lang: 'SPA'
//  },
//  {
//    key: 'lengthAudio', // Durata Audio FRA
//    lang: 'FRA'
//  },
//  {
//    key: 'lengthAudio', // Durata Audio DEU
//    lang: 'DEU'
//  },
//  'youtubeUrl',		// link video youtube
//  'video',		// Nome video
//  'text',		// Nome testo
//  'audio',		// Nome audio
//  'lat',		// Latitudine (formato WGS84)
//  'lon',		// Longitudine (formato WGS84)
//  '',			// Codice CDB (Non utilizzato)
//  '',			// Data OnLine (Non utilizzato)
//  //
//  // Eventi
//  //
//  'EventStartDate',	// Data inizio Evento
//  'EventEbdDate',	// Data fine Evento
//  {
//    key: 'AdvId',	// Id Inserzionista
//    type: 'int'
//  },
//  {
//    key: 'PosterId',	// Id Locandina
//    type: 'int'
//  },
//  //
//  // Sponsor
//  //
//  '',			// Non utilizzato
//  '',			// Non utilizzato
//  'WelcomeAudio',	// File Audio di Benvenuto
//  'LogoHomePage',	// Immagine della Home Page
//  'BannerMappa',	// Immagine del banner nella mappa
//  //
//  {
//    key: 'dataExists',	// Flag Esistenza File ITA
//    lang: 'ITA'
//  },
//  {
//    key: 'dataExists',	// Flag Esistenza File ENG
//    lang: 'ENG'
//  },
//  {
//    key: 'dataExists',	// Flag Esistenza File SPA
//    lang: 'SPA'
//  },
//  {
//    key: 'dataExists',	// Flag Esistenza File FRA
//    lang: 'FRA'
//  },
//  {
//    key: 'dataExists',	// Flag Esistenza File DEU
//    lang: 'DEU'
//  },
//  {
//    key: 'dataExists',	// Flag Esistenza File RUS
//    lang: 'RUS'
//  },
//  '',			// Non utilizzato
//  //
//  // Foto
//  //
//  'photo1',		// Nome foto #1
//  'photo2',		// Nome foto #2
//  'photo3'		// Nome foto #3
//];

//exports.listaItinerari = [
//  //
//  //Dati anagrafici
//  //
//  {
//    key:'_id',         //ID Itinerario
//    type: 'int'
//  },
//  {
//    key: 'name',      //Descrizione itinerario ITA
//    lang: 'ITA'
//  },
//  'autore',           //Autore dell'Itinerario (ad oggi esiste solo "Palmipedo")
//  'nazione',          //Nazione (non utilizzato)
//  {
//    key: 'comune',            //descrizione comune
//    type: 'uppercaseString'
//  },
//  {
//    key: 'lunghezza',        //Lunghezza itinerario (metri)
//    type: 'int'
//  },
//  {
//    key: 'nDurata',          //Durata Itinerario (minuti)
//    type: 'int'
//  },
//  'hDurata',          //Durata Itinerario (hh:mm:ss)
//  {
//    key: 'name',      //Descrizione itinerario ENG
//    lang: 'ENG'
//  },
//  {
//    key: 'name',      //Descrizione itinerario SPA
//    lang: 'SPA'
//  },
//  {
//    key: 'name',      //Descrizione itinerario FRA
//    lang: 'FRA'
//  },
//  {
//    key: 'name',      //Descrizione itinerario DEU
//    lang: 'DEU'
//  },
//  {
//    key: 'name',      //Descrizione itinerario RUS
//    lang: 'RUS'
//  },
//  '',
//  '',
//  '',
//  '',
//  {
//    key: 'nsiti',            //Numero di siti incontrati nel percorso
//    type: 'int'
//  },
//  'image',            //Immagine itinerario
//  'tipo',             //Tipo itinerario (A piedi, in auto, etc)
//  'indirizzopartenza',//Indirizzo di partenza
//  'indirizzoarrivo'   //Indirizzo di arrivo  
//];

//exports.estrazioneItinerario = [
//  //
//  //Dati anagrafici
//  //
//  {
//    key: 'itinerarioId', //ID itinerario
//    type: 'int'
//  },
//  {
//    key: '_id',        //ID tappa itinerario
//    type: 'int'
//  },
//  'lat',              //Latitudine (formato WGS84)
//  'lon',              //Longitudine (formato WGS84)
//  {
//    key: 'catId',     //Codice Categoria
//    type: 'int'
//  },
//  {
//    key: 'serviceId', //Codice Servizio
//    type: 'int'
//  },
//  {
//    key: 'schedaCode',//Codice Scheda
//    type: 'int'
//  },
//  {
//    key: 'name',      //Descrizione scheda ITA
//    lang: 'ITA'
//  },
//  {
//    key: 'name',      //Descrizione scheda ENG
//    lang: 'ENG'
//  },
//  {
//    key: 'name',      //Descrizione scheda SPA
//    lang: 'SPA'
//  },
//  {
//    key: 'name',      //Descrizione scheda FRA
//    lang: 'FRA'
//  },
//  {
//    key: 'name',      //Descrizione scheda DEU
//    lang: 'DEU'
//  },
//  {
//    key: 'name',      //Descrizione scheda RUS
//    lang: 'RUS'
//  },
//  {
//    key: 'ndistanza',        //Distanza dalla tappa precedente (metri)
//    type: 'int'
//  },
//  'hdurata'           //Durata percorrenza dalla tappa precedente (hh:mm:ss)
//];

