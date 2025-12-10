using System;
using System.Collections.Generic;
using System.Text;

namespace Palmipedo.Models
{
    public class Categoria
    {
        public int _id { get; set; }
        public string icon { get; set; }
        public string menuIcon { get; set; }
        public string color { get; set; }
        public string name { get; set; }

        public override bool Equals(object obj)
        {
            return this._id.Equals(((Models.Categoria)obj)._id);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
