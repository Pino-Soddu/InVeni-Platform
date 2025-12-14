using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inveni.App.Modelli
{
    public class Comune
    {
        public int id { get; set; }
        // <summary>
        // IdSponsor == 0 non ha sponsor
        // </summary>
        public int IdSponsor { get; set; }
        public string? DescrSponsor { get; set; }
        public string? nazione { get; set; }
        public string? name { get; set; }
        public string? image { get; set; }
        public int cityType { get; set; }
        public bool visible { get; set; }
        public bool autostart { get; set; }
        public string? catsInMap { get; set; }
        public string? catsDefault { get; set; }
        public string? langs { get; set; }
        public List<string> LangList
        {
            get
            {
                if (string.IsNullOrEmpty(langs)) return new List<string>();

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
                if (string.IsNullOrEmpty(catsInMap)) return new List<int>();

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
        public string? userMap { get; set; }
        public double SO_Lat { get; set; }
        public double SO_Lng { get; set; }
        public double NE_Lat { get; set; }
        public double NE_Lng { get; set; }
        public int raggio { get; set; }
        public int Orientamento { get; set; }
        public int Profondita { get; set; }
        public int aggregazione { get; set; }
        public string? lengthAudio { get; set; }
        public double mbAudio { get; set; }
        public double mbPhotos { get; set; }
        public double mbTexts { get; set; }
        public int nAudio { get; set; }
        public int nPhotos { get; set; }
        public int nTexts { get; set; }
    }
}
