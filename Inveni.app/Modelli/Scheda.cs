using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Palmipedo.Models
{
    public class Scheda
    {
        public int _id { get; set; }
        public string AdvId { get; set; }
        public string BannerMappa { get; set; }
        public DateTime? EventEndDate { get; set; }
        public DateTime? EventStartDate { get; set; }
        public string ImgAvvio { get; set; }
        public string ImgHomePage { get; set; }
        public int LocandinaId { get; set; }
        public string pathText { get; set; }
        public string ShortName { get; set; }
        public int SponsorId { get; set; }
        //public string LogoHomePage { get; set; }
        //public int PosterId { get; set; }
        public string Telephone { get; set; }
        public string WelcomeAudio { get; set; }
        public string address { get; set; }
        public string audio { get; set; }
        public int catId { get; set; }
        public string comune { get; set; }
        public string dataExists { get; set; }
        public string date { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string lengthAudio { get; set; }
        public int nClicks { get; set; }
        public int nListens { get; set; }
        public int nVisits { get; set; }
        public string name { get; set; }
        public string photo1 { get; set; }
        public string photo2 { get; set; }
        public string photo3 { get; set; }
        public int priority { get; set; }
        public int radius { get; set; }
        public int schedaCode { get; set; }
        public int serviceId { get; set; }
        public string text { get; set; }
        public int userId { get; set; }
        public string video { get; set; }
        public bool visible { get; set; }
        public string youtubeUrl { get; set; }
        public string TestoEnigma { get; set; }
        public string LunghezzaCaccia { get; set; }
        public string NumTappeCaccia { get; set; }
        public bool IsVisibleByFilter { get; set; }
        public DateTime? LastListeningDate { get; set; }
        public string AreaEnigmaMp3 { get; set; }

        [JsonIgnore]
        public List<string> Photos
        {
            get
            {
                List<string> list = new List<string>();
                list.Add(photo1);
                list.Add(photo2);
                list.Add(photo3);
                return list;
            }
        }

        [JsonIgnore]
        public bool IsTreasureHuntItem
        {
            get
            {
                return (catId == 20 && (serviceId == 1
                                        || serviceId == 2
                                        || serviceId == 3
                                        || serviceId == 4
                                        || serviceId == 5
                                        || serviceId == 6));
            }
        }

        [JsonIgnore]
        public HuntType HuntType
        {
            get
            {
                if (catId == 20 && serviceId == 4)
                {
                    return HuntType.INTERMEDIATE;
                }
                else if (catId == 20 && serviceId == 5)
                {
                    return HuntType.ADVANCED;
                }
                else
                {
                    return HuntType.NULL;
                }
            }
        }

        [JsonIgnore]
        public EnigmaType EnigmaType
        {
            get
            {
                if (catId == 20 && serviceId == 2)
                {
                    return EnigmaType.INDIZIO;
                }
                else if (catId == 20 && serviceId == 3)
                {
                    return EnigmaType.ENIGMA;
                }
                else
                {
                    return EnigmaType.NULL;
                }
            }
        }

        [JsonIgnore]
        public ItemItinerario ItemItinerario { get; set; }
    }

    public enum EnigmaType
    {
        NULL,
        INDIZIO,
        ENIGMA
    }

    public enum HuntType
    {
        NULL,
        INTERMEDIATE,
        ADVANCED
    }
}
