using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palmipedo.Models
{
    //public class ComuneOLD
    //{
    //    public string Nazione { get; set; }
    //    public string DescrCitta { get; set; }
    //    /// <summary>
    //    /// Latitudine Centro
    //    /// </summary>
    //    public double LatC { get; set; }
    //    /// <summary>
    //    /// Longitudine Centro
    //    /// </summary>
    //    public double LngC { get; set; }
    //    public double ZoomCartina { get; set; }
    //    public string MappaUtente { get; set; }
    //    public double SO_Lat { get; set; }
    //    public double SO_Lng { get; set; }
    //    public double NE_Lat { get; set; }
    //    public double NE_Lng { get; set; }
    //    public string CategdaMappare { get; set; }
    //    public string Traduzioni { get; set; } //int?
    //    public string CategAccese { get; set; } //int?
    //    public string ImmagineScelta { get; set; }
    //    public int NumSK { get; set; }
    //    public int NumAudio_ITA { get; set; }
    //    public int NumFoto { get; set; }
    //    public int NumTesti_Ita { get; set; }
    //    public int DurataAudio_ITA { get; set; }
    //    public double MBFoto { get; set; }
    //    public double MBTesti_Ita { get; set; }
    //    public double MBTesti_Eng { get; set; }
    //    public double MBTesti_Spa { get; set; }
    //    public double MBTesti_Fra { get; set; }
    //    public double MBTesti_Ger { get; set; }
    //    public double MBTesti_Rus { get; set; }
    //    public double MBAudio_Ita { get; set; }
    //    public double MBAudio_Eng { get; set; }
    //    public double MBAudio_Spa { get; set; }
    //    public double MBAudio_Fra { get; set; }
    //    public double MBAudio_Ger { get; set; }
    //    public double MBAudio_Rus { get; set; }
    //    public bool Visibilita { get; set; }
    //    public int TipoCitta { get; set; }
    //    public string ImageStemma { get; set; }
    //    public string LinkRSS { get; set; }
    //    public int IdUtParlaColSindaco { get; set; }
    //    public string Orientamento { get; set; }
    //    public string Profondita { get; set; }
    //    public int RaggioDintorniKM { get; set; }
    //    public int Aggregazione { get; set; }
    //    public bool Municipio { get; set; }
    //    public bool AutoStart { get; set; }
    //    public int NumTesti_Eng { get; set; }
    //    public int NumTesti_Spa { get; set; }
    //    public int NumTesti_Fra { get; set; }
    //    public int NumTesti_Ger { get; set; }
    //    public int NumTesti_Rus { get; set; }
    //    public int NumAudio_Eng { get; set; }
    //    public int NumAudio_Spa { get; set; }
    //    public int NumAudio_Fra { get; set; }
    //    public int NumAudio_Ger { get; set; }
    //    public int NumAudio_Rus { get; set; }
    //    public string DurataAudio_Eng { get; set; }
    //    public string DurataAudio_Spa { get; set; }
    //    public string DurataAudio_Fra { get; set; }
    //    public string DurataAudio_Ger { get; set; }
    //    public string DurataAudio_Rus { get; set; }
    //    public int NumFoto_Eng { get; set; }
    //    public int NumFoto_Spa { get; set; }
    //    public int NumFoto_Fra { get; set; }
    //    public int NumFoto_Ger { get; set; }
    //    public int NumFoto_Rus { get; set; }
    //}

    public class Comune
    {
        public int id { get; set; }
        /// <summary>
        /// IdSponsor == 0 non ha sponsor
        /// </summary>
        public int IdSponsor { get; set; }
        public string DescrSponsor { get; set; }
        public string nazione { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public int cityType { get; set; }
        public bool visible { get; set; }
        public bool autostart { get; set; }
        public string catsInMap { get; set; }
        public string catsDefault { get; set; }
        public string langs { get; set; }
        public List<string> LangList
        {
            get
            {
                string[] splitted = langs.Split(';');
                List<string> list = new List<string>();
                for (int i = 0; i < splitted.Length; i++)
                {
                    if (!string.IsNullOrEmpty(splitted[i]))
                        list.Add(splitted[i]);
                }
                return list;
            }
        }
        public List<int> CategoriesInMap
        {
            get
            {
                List<int> list = new List<int>();
                string[] splitted = catsInMap.Split(';');
                foreach (var item in splitted)
                {
                    if (!string.IsNullOrEmpty(item))
                        list.Add(int.Parse(item.Split('-')[0]));
                }
                return list.Distinct().ToList();
            }
        }
        public int nItinerari { get; set; }
        public int nSchede { get; set; }
        public double latC { get; set; }
        public double lonC { get; set; }
        public decimal zoom { get; set; }
        public string userMap { get; set; }
        public double SO_Lat { get; set; }
        public double SO_Lng { get; set; }
        public double NE_Lat { get; set; }
        public double NE_Lng { get; set; }
        public int raggio { get; set; }
        public int Orientamento { get; set; }
        public int Profondita { get; set; }
        public int aggregazione { get; set; }
        public string lengthAudio { get; set; }
        public double mbAudio { get; set; }
        public double mbPhotos { get; set; }
        public double mbTexts { get; set; }
        public int nAudio { get; set; }
        public int nPhotos { get; set; }
        public int nTexts { get; set; }
    }
}
