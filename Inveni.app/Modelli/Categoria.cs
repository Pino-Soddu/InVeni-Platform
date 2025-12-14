using System;
using System.Collections.Generic;
using System.Text;

namespace Inveni.App.Modelli
{
    public class Categoria
    {
        public int _id { get; set; }
        public string? icon { get; set; }
        public string? menuIcon { get; set; }
        public string? color { get; set; }
        public string? name { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Categoria categoria)
                return this._id.Equals(categoria._id);
            return false;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
