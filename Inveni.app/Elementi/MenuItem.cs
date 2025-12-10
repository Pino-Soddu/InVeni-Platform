using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Palmipedo.iOS.Core.Entities
{
    public class MenuItem<T>
    {
        public string Title { get; set; }
        public T CustomUserObject { get; set; }
    }
}