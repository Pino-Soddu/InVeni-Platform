using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.IO;
using System.Threading.Tasks;

namespace Palmipedo.iOS.Core.Entities
{
    public static class Common
    {
        public static string LANGUAGE_ITALIANO = "Italiano";
        public static string LANGUAGE_ITALIANO_NATIVE = "Italiano";
        public static string LANGUAGE_ITALIANO_ABBR = "ita";
        public static string LANGUAGE_ITALIANO_CULTURE = "it-it";
        public static string LANGUAGE_ITALIANO_FLAG = "Flag_Italian";

        public static string LANGUAGE_INGLESE = "Inglese";
        public static string LANGUAGE_INGLESE_NATIVE = "English";
        public static string LANGUAGE_INGLESE_ABBR = "eng";
        public static string LANGUAGE_INGLESE_CULTURE = "en-us";
        public static string LANGUAGE_INGLESE_FLAG = "Flag_Us";

        public static string LANGUAGE_SPAGNOLO = "Spagnolo";
        public static string LANGUAGE_SPAGNOLO_NATIVE = "Espanol";
        public static string LANGUAGE_SPAGNOLO_ABBR = "esp";
        public static string LANGUAGE_SPAGNOLO_CULTURE = "es-es";
        public static string LANGUAGE_SPAGNOLO_FLAG = "Flag_Spanish";

        public static string LANGUAGE_FRANCESE = "Francese";
        public static string LANGUAGE_FRANCESE_NATIVE = "Francais";
        public static string LANGUAGE_FRANCESE_ABBR = "fra";
        public static string LANGUAGE_FRANCESE_CULTURE = "fr-fr";
        public static string LANGUAGE_FRANCESE_FLAG = "Flag_French";

        public static string LANGUAGE_TEDESCO = "Tedesco";
        public static string LANGUAGE_TEDESCO_NATIVE = "Deutsch";
        public static string LANGUAGE_TEDESCO_ABBR = "deu";
        public static string LANGUAGE_TEDESCO_CULTURE = "de-de";
        public static string LANGUAGE_TEDESCO_FLAG = "Flag_German";

        public static string LANGUAGE_RUSSO = "Russo";
        public static string LANGUAGE_RUSSO_NATIVE = "Pусский";
        public static string LANGUAGE_RUSSO_ABBR = "rus";
        public static string LANGUAGE_RUSSO_CULTURE = "ru-ru";
        public static string LANGUAGE_RUSSO_FLAG = "Flag_Russian";

        public static int TOTAL_LANGUAGES_NUMBER = 6;
    }

    public class ServiceImage
    {
        //public static async Task<UIImage> GetFromServiceCodeRemote(Models.Scheda card)
        //{
        //    //string.Format(IndexViewController.URL_BASE_PREVIEW_CARD, Uri.EscapeUriString(card.comune), Uri.EscapeUriString(card.BannerMappa))
        //    return await Utils.GetUIImageFromUrlAsync("http://www.palmipedo.info/Palmipedo/ArchivioCitta/Frascati/Foto/13384-BannerMap.png"); //
        //}

        public static UIImage GetFromServiceCode(string mapIcon)
        {
            if (!string.IsNullOrEmpty(mapIcon))
            {
                string bundleName = "Marker_";
                var tmp = mapIcon.Replace("100", "");
                bundleName += Path.GetFileNameWithoutExtension(tmp);

                UIImage tmpImage = UIImage.FromBundle(bundleName);
                if (tmpImage != null)
                    return tmpImage;
                else
                    return UIImage.FromBundle("Marker_Manca");
            }
            else
            {
                return UIImage.FromBundle("Marker_Manca");
            }

            //if ((categoryCode == 11 && serviceCode == 7)
            //    || (categoryCode == 17 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_Piazze");
            //}
            //else if ((categoryCode == 11 && serviceCode == 2)
            //    || (categoryCode == 17 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Chiesa");
            //}
            //else if ((categoryCode == 11 && serviceCode == 9)
            //   || (categoryCode == 19 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Montagne");
            //}
            //else if ((categoryCode == 11 && serviceCode == 6)
            //    || (categoryCode == 17 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Musei");
            //}
            //else if ((categoryCode == 11 && serviceCode == 8)
            //    || (categoryCode == 14 && serviceCode == 1)
            //    || (categoryCode == 17 && serviceCode == 6))
            //{
            //    return UIImage.FromBundle("Marker_Storie");
            //}
            //else if ((categoryCode == 11 && serviceCode == 1)
            //   || (categoryCode == 17 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Monumenti");
            //}
            //else if ((categoryCode == 12 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Sponsor");
            //}
            //else if ((categoryCode == 13 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_News");
            //}
            //else if ((categoryCode == 13 && serviceCode == 3)
            //    || (categoryCode == 15 && serviceCode == 10))
            //{
            //    return UIImage.FromBundle("Marker_Trasporti");
            //}
            //else if ((categoryCode == 13 && serviceCode == 4)
            //    || (categoryCode == 13 && serviceCode == 7))
            //{
            //    return UIImage.FromBundle("Marker_Bus");
            //}
            //else if ((categoryCode == 13 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_Metro");
            //}
            //else if ((categoryCode == 13 && serviceCode == 6))
            //{
            //    return UIImage.FromBundle("Marker_TitoliViaggio");
            //}
            //else if ((categoryCode == 13 && serviceCode == 8))
            //{
            //    return UIImage.FromBundle("Marker_Cotral");
            //}
            //else if ((categoryCode == 13 && serviceCode == 9))
            //{
            //    return UIImage.FromBundle("Marker_Fermata");
            //}
            //else if ((categoryCode == 15 && serviceCode == 1)
            //    || (categoryCode == 15 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_Eventi");
            //}
            //else if ((categoryCode == 15 && serviceCode == 2)
            //    || (categoryCode == 16 && serviceCode == 6)
            //    || (categoryCode == 61 && serviceCode == 12))
            //{
            //    return UIImage.FromBundle("Marker_Ristoranti");
            //}
            //else if ((categoryCode == 15 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Dormire");
            //}
            //else if ((categoryCode == 15 && serviceCode == 4))
            //{
            //    return UIImage.FromBundle("Marker_Itinerari");
            //}
            //else if ((categoryCode == 15 && serviceCode == 5)
            //    || (categoryCode == 52 && serviceCode == 1)
            //    || (categoryCode == 61 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_Moda");
            //}
            //else if ((categoryCode == 15 && serviceCode == 7))
            //{
            //    return UIImage.FromBundle("Marker_Assoc");
            //}
            //else if ((categoryCode == 15 && serviceCode == 8)
            //    || (categoryCode == 21 && serviceCode == 3)
            //    || (categoryCode == 60 && serviceCode == 7)
            //    || (categoryCode == 61 && serviceCode == 13)
            //    || (categoryCode == 63 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Sanita");
            //}
            //else if ((categoryCode == 15 && serviceCode == 9)
            //    || (categoryCode == 61 && serviceCode == 8)
            //    || (categoryCode == 63 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Tempolibero");
            //}
            //else if ((categoryCode == 15 && serviceCode == 11)
            //    || (categoryCode == 21 && serviceCode == 1)
            //    || (categoryCode == 60 && serviceCode == 6)
            //    || (categoryCode == 61 && serviceCode == 10)
            //    || (categoryCode == 63 && serviceCode == 7))
            //{
            //    return UIImage.FromBundle("Marker_Banca");
            //}
            //else if ((categoryCode == 15 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Campeggi");
            //}
            //else if ((categoryCode == 15 && serviceCode == 13))
            //{
            //    return UIImage.FromBundle("Marker_Stabilimenti");
            //}
            //else if ((categoryCode == 15 && serviceCode == 14)
            //    || (categoryCode == 52 && serviceCode == 6))
            //{
            //    return UIImage.FromBundle("Marker_Parcheggio");
            //}
            //else if ((categoryCode == 15 && serviceCode == 15)
            //    || (categoryCode == 21 && serviceCode == 4))
            //{
            //    return UIImage.FromBundle("Marker_Poste");
            //}
            //else if ((categoryCode == 15 && serviceCode == 16)
            //    || (categoryCode == 21 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_PubSic");
            //}
            //else if ((categoryCode == 15 && serviceCode == 17)
            //    || (categoryCode == 21 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Farmacia");
            //}
            //else if ((categoryCode == 15 && serviceCode == 18))
            //{
            //    return UIImage.FromBundle("Marker_RaccoltaDiff");
            //}
            //else if ((categoryCode == 16 && serviceCode == 1)
            //    || (categoryCode == 18 && serviceCode == 1)
            //    || (categoryCode == 18 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Tipiche");
            //}
            //else if ((categoryCode == 16 && serviceCode == 2)
            //    || (categoryCode == 18 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Enoteca");
            //}
            //else if ((categoryCode == 16 && serviceCode == 3)
            //    || (categoryCode == 16 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_AzAgricola");
            //}
            //else if ((categoryCode == 16 && serviceCode == 4)
            //    || (categoryCode == 18 && serviceCode == 4)
            //    || (categoryCode == 60 && serviceCode == 4)
            //    || (categoryCode == 63 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Ristoranti2");
            //}
            //else if ((categoryCode == 17 && serviceCode == 9))
            //{
            //    return UIImage.FromBundle("Marker_Gemellaggio");
            //}
            //else if ((categoryCode == 19 && serviceCode == 1)
            //    || (categoryCode == 19 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Parchi");
            //}
            //else if ((categoryCode == 19 && serviceCode == 4)
            //    || (categoryCode == 19 && serviceCode == 5)
            //    || (categoryCode == 19 && serviceCode == 6)
            //    || (categoryCode == 19 && serviceCode == 7)
            //    || (categoryCode == 19 && serviceCode == 8))
            //{
            //    return UIImage.FromBundle("Marker_Spiagge");
            //}
            //else if ((categoryCode == 20 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_ChiesaRupestre");
            //}
            //else if ((categoryCode == 30 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Cluster");
            //}
            //else if ((categoryCode == 30 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Aaree");
            //}
            //else if ((categoryCode == 30 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Sito");
            //}
            //else if ((categoryCode == 44 && serviceCode == 1)
            //    || (categoryCode == 52 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_Locandina");
            //}
            //else if ((categoryCode == 44 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Oggi");
            //}
            //else if ((categoryCode == 44 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Settimana");
            //}
            //else if ((categoryCode == 44 && serviceCode == 4))
            //{
            //    return UIImage.FromBundle("Marker_Mese");
            //}
            //else if ((categoryCode == 44 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_Prossimamente");
            //}
            //else if ((categoryCode == 45 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Media");
            //}
            //else if ((categoryCode == 50 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Madonnella");
            //}
            //else if ((categoryCode == 50 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_Franz");
            //}
            //else if ((categoryCode == 52 && serviceCode == 2)
            //   || (categoryCode == 61 && serviceCode == 11))
            //{
            //    return UIImage.FromBundle("Marker_Album");
            //}
            //else if ((categoryCode == 52 && serviceCode == 3)
            //   || (categoryCode == 61 && serviceCode == 4))
            //{
            //    return UIImage.FromBundle("Marker_Casa");
            //}
            //else if ((categoryCode == 52 && serviceCode == 4)
            //    || (categoryCode == 60 && serviceCode == 5)
            //    || (categoryCode == 61 && serviceCode == 1)
            //    || (categoryCode == 61 && serviceCode == 2)
            //    || (categoryCode == 61 && serviceCode == 3)
            //    || (categoryCode == 61 && serviceCode == 6)
            //    || (categoryCode == 63 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Shopping");
            //}
            //else if ((categoryCode == 60 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Attrazioni");
            //}
            //else if ((categoryCode == 60 && serviceCode == 2)
            //    || (categoryCode == 61 && serviceCode == 9))
            //{
            //    return UIImage.FromBundle("Marker_Cosafare");
            //}
            //else if ((categoryCode == 60 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Bar");
            //}
            //else if ((categoryCode == 60 && serviceCode == 8)
            //    || (categoryCode == 63 && serviceCode == 4))
            //{
            //    return UIImage.FromBundle("Marker_PuntoFoto");
            //}
            //else if ((categoryCode == 60 && serviceCode == 9))
            //{
            //    return UIImage.FromBundle("Marker_Chiave");
            //}
            //else if ((categoryCode == 60 && serviceCode == 10)
            //    || (categoryCode == 63 && serviceCode == 8))
            //{
            //    return UIImage.FromBundle("Marker_Toilette");
            //}
            //else if ((categoryCode == 61 && serviceCode == 7))
            //{
            //    return UIImage.FromBundle("Marker_Estetica");
            //}
            //else if ((categoryCode == 62 && serviceCode == 1))
            //{
            //    return UIImage.FromBundle("Marker_Magicland");
            //}
            //else if ((categoryCode == 62 && serviceCode == 2)
            //    || (categoryCode == 64 && serviceCode == 4))
            //{
            //    return UIImage.FromBundle("Marker_AttrCoragg");
            //}
            //else if ((categoryCode == 62 && serviceCode == 3)
            //    || (categoryCode == 64 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_AttrTutti");
            //}
            //else if ((categoryCode == 62 && serviceCode == 4)
            //    || (categoryCode == 64 && serviceCode == 6))
            //{
            //    return UIImage.FromBundle("Marker_AttrBambini");
            //}
            //else if ((categoryCode == 62 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_SpettAnim");
            //}
            //else if ((categoryCode == 63 && serviceCode == 5))
            //{
            //    return UIImage.FromBundle("Marker_PuntoInfo");
            //}
            //else if ((categoryCode == 63 && serviceCode == 9))
            //{
            //    return UIImage.FromBundle("Marker_Picnic");
            //}
            //else if ((categoryCode == 64 && serviceCode == 1)
            //    || (categoryCode == 64 && serviceCode == 2))
            //{
            //    return UIImage.FromBundle("Marker_CineW");
            //}
            //else if ((categoryCode == 64 && serviceCode == 3))
            //{
            //    return UIImage.FromBundle("Marker_Adrenalina");
            //}

            //return UIImage.FromBundle("Marker_Manca");
        }
    }
}

//CAT     SER
//OK 11	    1	CITTA STORICA                   50Monumenti.png	        100Monumenti.png
//OK 11	    2	CHIESE E LUOGHI DI CULTO	    50Chiesa.png	        100Chiesa.png
//OK 11	    5	FONTANE GIARDINI  E VILLE       50Parchi.png	        100Parchi.png
//OK 11	    6	MUSEI PALAZZI E CASE            50Musei.png	            100Musei.png
//OK 11	    7	PIAZZE STRADE E CONTRADE        50piazze.png	        100piazze.png
//OK 11	    8	APPROFONDIMENTI STORICI         50Storie.png	        100Storie.png
//OK 11	    9	AMBIENTI NATURALISTICI          50Montagne.png	        100Montagne.png
//OK 12	    1	SPONSOR	                        50Sponsor.png	        100Sponsor.png
//OK 13	    1	NOTIZIE GENERALI                50news.png	            100news.png
//OK 13	    3	TRENI FS                        50trasporti.png	        100treno.png
//OK 13	    4	BUS	                            50Bus.png	            100Bus.png
//OK 13	    5	METRO	                        50Metro.png	            100Metro.png
//OK 13	    6	RIVENDITA TITOLI DI VIAGGIO     50TitoliViaggio.png	    100TitoliViaggio.png
//OK 13	    7	AUTOLINEE URBANE                50Bus.png	            100Bus.png
//OK 13	    8	AUTOLINEE EXTRA-URBANE	        50Cotral.png	        100Cotral.png
//OK 13	    9	FERMATE	                        50Fermata.png	        100Fermata.png
//OK 14	    1	IL PALIO                        50Storie.png	        100Storie.png
//OK 15	    1	NOTIZIE GENERALI                50eventi.png	        100eventi.png
//OK 15	    2	DOVE MANGIARE                   50Ristoranti.png	    100Ristoranti.png
//OK 15	    3	DOVE DORMIRE                    50dormire.png	        100dormire.png
//OK 15	    4	COSA FARE                       50Itinerari.png	        100Itinerari.png
//OK 15	    5	FESTE EVENTI E TRADIZIONI       50eventi.png	        100eventi.png
//OK 15	    6	SHOPPING E DIVERTIMENTO	        50Moda.png	            100Moda.png
//OK 15	    7	SERVIZI PUBBLICI                50assoc.png	            100assoc.png
//OK 15	    8	SERVIZI SANITARI                50Sanita.png	        100Sanita.png
//OK 15	    9	SPETTACOLI E TEMPO LIBERO       50Tempolibero.png	    100Tempolibero.png
//OK 15	    10	TRASPORTO PUBBLICO              50trasporti.png	        100trasporti.png
//OK 15	    11	BANCHE	                        50Banca.png	            100Banca.png
//OK 15	    12	CAMPING & VILLAGGI TURISTICI    50Campeggi.png	        100Campeggi.png
//OK 15	    13	STABILIMENTI BALNEARI           50Stabilimenti.png	    100Stabilimenti.png
//OK 15	    14	PARCHEGGI	                    50parcheggio.png	    100parcheggio.png
//OK 15	    15	UFFICI POSTALI                  50Poste.png	            100Poste.png
//OK 15	    16	PUBBLICA SICUREZZA              50PubSic.png	        100PubSic.png
//OK 15	    17	FARMACIE	                    50Farmacia.png	        100Farmacia.png
//OK 15	    18	ISOLE ECOLOGICHE                50RaccoltaDiff.png	    100RaccoltaDiff.png
//OK 16	    1	PRODOTTI TIPICI                 50tipiche.png	        100tipiche.png
//OK 16	    2	CANTINE E ENOTECHE	            50enoteca.png	        100enoteca.png
//OK 16	    3	AZIENDE AGRICOLE                50AzAgricola.png	    100AzAgricola.png
//OK 16	    4	RICETTE TIPICHE                 50ristoranti2.png	    100ristoranti2.png
//OK 16	    5	AGRITURISMO	                    50AzAgricola.png	    100AzAgricola.png
//OK 16	    6	DOVE MANGIARE                   50Ristoranti.png	    100Ristoranti.png
//OK 17	    1	CHIESE E LUOGHI DI CULTO	    50Chiesa.png	        100Chiesa.png
//OK 17	    2	MONUMENTI E SITI ARCHEOLOGICI   50Monumenti.png	        100Monumenti.png
//OK 17	    3	MUSEI PALAZZI E VILLE           50Musei.png	            100Musei.png
//OK 17	    5	PIAZZE, STRADE E CONTRADE	    50piazze.png	        100piazze.png
//OK 17	    6	RACCONTI, STORIE E LEGGENDE	    50Storie.png	        100Storie.png
//OK 17	    9	GEMELLAGGI	                    50Gemellaggio.png	    100Gemellaggio.png
//OK 18	    1	PANE DI GENZANO IGP             50tipiche.png	        100tipiche.png
//OK 18	    2	PANE DI LARIANO	                50tipiche.png	        100tipiche.png
//OK 18	    3	VINO CASTELLI ROMANI DOC        50enoteca.png	        100enoteca.png
//OK 18	    4	PORCHETTA DI ARICCIA IGP        50ristoranti2.png	    100ristoranti2.png
//OK 19	    1	PARCHI E GIARDINI	            50Parchi.png	        100Parchi.png
//OK 19	    2	AREE NATURALISTICHE             50Montagne.png	        100Montagne.png
//OK 19	    3	PARCHI FAUNISTICI               50Parchi.png	        100Parchi.png
//OK 19	    4	SPIAGGE	                        50Spiagge.png	        100Spiagge.png
//OK 19	    5	MONTI	                        50Spiagge.png	        100Montagne.png             ->errore
//OK 19	    6	FIUMI E LAGHI	                50Spiagge.png	        100Montagne.png             ->errore
//OK 19	    7	SENTIERI	                    50Spiagge.png	        100Parchi.png               ->errore
//OK 19	    8	PISTE CICLABILI                 50Spiagge.png	        100Itinerari                ->errore
//OK 20	    1	CHIESE RUPESTRI                 50ChiesaRupestre.png	100ChiesaRupestre.png
//OK 21	    1	BANCHE	                        50Banca.png	            100Banca.png
//OK 21	    2	FARMACIE	                    50Farmacia.png	        100Farmacia.png
//OK 21	    3	SERVIZI SANITARI                50Sanita.png	        100Sanita.png
//OK 21	    4	UFFICI POSTALI                  50Poste.png	            100Poste.png
//OK 21	    5	PUBBLICA SICUREZZA              50PubSic.png	        100PubSic.png
//OK 30	    1	CLUSTER	                        50Cluster.png	        100Cluster.png
//OK 30	    2	AREE TEMATICHE                  50Aaree.png	            100Aree.png
//OK 30	    3	SITO ESPOSITIVO                 50Sito.png	            100Sito.png
//OK 44	    1	LOCANDINE	                    50Locandina.png	        100Locandina.png
//OK 44	    2	OGGI	                        50oggi.png	            100oggi.png
//OK 44	    3	QUESTA SETTIMANA                50settimana.png	        100settimana.png
//OK 44	    4	QUESTO MESE                     50mese.png	            100mese.png
//OK 44	    5	PROSSIMAMENTE	                50Prossimamente.png	    100Prossimamente.png
//OK 45	    1	MEDIA	                        50media.png	            100media.png
//OK 50	    1	LE MADONNELLE DI ROMA           50Madonnella.png	    100Madonnella.png
//OK 50	    2	ROMA SPARITA - FRANZ	        50Franz.png	            100Franz.png
//OK 52	    1	COMPAGNI DI VIAGGIO	            50Moda.png	            100Moda.png                 ->errore?
//OK 52	    2	APPUNTI	                        50album.png	            100album.png
//OK 52	    3	CURIOSITA'	                    50Casa.png	            100Casa.png                 ->errore?
//OK 52	    4	SHOPPING	                    50shopping.png	        100shopping.png
//OK 52	    5	SPETTACOLI	                    50Locandina.png	        100Locandina.png
//OK 52	    6	PARCHEGGIO AUTOMOBILE           50parcheggio.png	    100parcheggio.png
//OK 60	    1	ATTRAZIONI	                    50Attrazioni.png	    100AttrCoragg.png
//OK 60	    2	INFO	                        50cosafare.png	        100cosafare.png
//OK 60	    3	BAR	                            50Bar.png	            100Bar.png
//OK 60	    4	RISTORANTI	                    50ristoranti2.png	    100ristoranti2.png
//OK 60	    5	SHOP	                        50shopping.png	        100shopping.png
//OK 60	    6	BANCOMAT	                    50Banca.png	            100Banca.png
//OK 60	    7	INFERMERIA	                    50Sanita.png	        100Sanita.png
//OK 60	    8	TOUR/FOTO	                    50PuntoFoto.png	        100PuntoFoto.png
//OK 60	    9	ARMADIETTI	                    50Chiave.png	        100Chiave.png
//OK 60	    10	TOILETTE	                    50Toilette.png	        100Toilette.png
//OK 61	    1	ABBIGLIAMENTO UOMO/DONNA	    50shopping.png	        100shopping.png
//OK 61	    2	ABBIGLIAMENTO INTIMO            50shopping.png	        100shopping.png
//OK 61	    3	ABBIGLIAMENTO SPORTIVO          50shopping.png	        100shopping.png
//OK 61	    4	CASA	                        50Casa.png	            100Casa.png
//OK 61	    5	CALZATURE E ACCESSORI	        50Moda.png	            100Moda.png
//OK 61	    6	ABBIGLIAMENTO BAMBINO           50shopping.png	        100shopping.png
//OK 61  	7	COSMETICI E GIOIELLERIA	        50Estetica.png	        100Estetica.png
//OK 61	    8	PUNTI DI RISTORO	            50Tempolibero.png	    100Tempolibero.png
//OK 61	    9	INFO	                        50cosafare.png	        100cosafare.png
//OK 61	    10	BANCOMAT	                    50Banca.png	            100Banca.png
//OK 61	    11	LA PICCOLA SARTORIA	            50album.png	            100album.png                ->errore
//OK 61	    12	TAXI	                        50Ristoranti.png	    100Ristoranti.png           ->errore
//OK 61	    13	TOILETTE - FASCIATOI	        50Sanita.png	        100Sanita.png
//OK 62	    1	IL PARCO                        50Magicland.png	        100Magicland.png
//OK 62	    2	ATTRAZIONI PER CORAGGIOSI	    50AttrCoragg.png	    100AttrCoragg.png
//OK 62  	3	ATTRAZIONI PER TUTTI	        50AttrTutti.png	        100AttrTutti.png
//OK 62	    4	ATTRAZIONI PER BAMBINI	        50AttrBambini.png	    100AttrBambini.png
//OK 62  	5	SPETTACOLI E ANIMAZIONI	        50SpettAnim.png	        100SpettAnim.png
//OK 63	    1	RISTORANTI	                    50ristoranti2.png	    100ristoranti2.png
//OK 63	    2	BAR-CHIOSCHI	                50Tempoliberopng	    100Bar.png
//OK 63	    3	SHOPPING	                    50shopping.png	        100shopping.png
//OK 63	    4	PUNTI FOTO                      50PuntoFoto.png	        100PuntoFoto.png
//OK 63	    5	UFFICIO INFORMAZIONI            50PuntoInfo.png	        100PuntoInfo.png
//OK 63	    6	INFERMERIA	                    50Sanita.png	        100Sanita.png
//OK 63     7	BANCOMAT	                    50Banca.png	            100Banca.png
//OK 63	    8	TOILETTE	                    50Toilette.png	        100Toilette.png
//OK 63	    9	AREA PIC-NIC                    RWM_Picnic.png          RWM_Picnic.png
//OK 64	    1	IL PARCO	                    50CineW.png             100CineW.png
//OK 64	    2	SET                             50CineW.png             100CineW.png
//OK 64	    3	ADRENALINA                      50Adrenalina.png        100Adrenalina.png
//OK 64	    4	AVVENTURA                       50AttrCoragg.png        100AttrCoragg.png
//OK 64	    5	PER TUTTA LA FAMIGLIA	        50AttrTutti.png         100AttrTutti.png
//OK 64	    6	PER I PIU PICCOLI	            50AttrBambini.png       100AttrBambini.png