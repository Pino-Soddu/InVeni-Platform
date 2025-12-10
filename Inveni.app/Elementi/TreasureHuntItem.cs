using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core.Entities
{
    public class TreasureHuntItem
    {
        public Guid Id { get; set; }
        public int IdGroup { get; set; }
        //public List<Models.ItemItinerario> ItemsItinerario { get; set; }
        //public List<Models.ItemItinerario> PointsTo { get; set; }
        public Models.Scheda IndizioEnigma { get; set; }
        public Models.Scheda Caccia { get; set; }
        public int Index { get; set; }

        //public HuntType HuntType
        //{
        //    get
        //    {
        //        if (Caccia.catId == 20 && Caccia.serviceId == 4)
        //        {
        //            return HuntType.INTERMEDIATE;
        //        }
        //        else if (Caccia.catId == 20 && Caccia.serviceId == 5)
        //        {
        //            return HuntType.ADVANCED;
        //        }
        //        else
        //        {
        //            return HuntType.NULL;
        //        }
        //    }
        //}

        //public EnigmaType EnigmaType
        //{
        //    get
        //    {
        //        if (Caccia.catId == 20 && Caccia.serviceId == 2)
        //        {
        //            return EnigmaType.INDIZIO;
        //        }
        //        else if (Caccia.catId == 20 && Caccia.serviceId == 3)
        //        {
        //            return EnigmaType.ENIGMA;
        //        }
        //        else
        //        {
        //            return EnigmaType.NULL;
        //        }
        //    }
        //}

        public TreasureHuntItem()
        {
            Id = Guid.NewGuid();
            //ItemsItinerario = new List<Models.ItemItinerario>();
            //PointsTo = new List<Models.ItemItinerario>();
        }
    }
}