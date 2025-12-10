using System;
using System.Collections.Generic;
using System.Text;

namespace Palmipedo.Models
{
    public class Servizio
    {
        public int catId { get; set; }
        public int serviceCode { get; set; }
        //public string icoMenu { get; set; }
        public string icoMap { get; set; }
        public string name { get; set; }

        //Custom
        public bool selected { get; set; }

        public Servizio()
        {
            selected = true;
        }
    }
}
