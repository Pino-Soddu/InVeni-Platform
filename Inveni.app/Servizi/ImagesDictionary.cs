using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.Xml;
using System.Threading.Tasks;

namespace Palmipedo.iOS.Core
{
    public class ImagesDictionary<T> : Dictionary<T, UIImage>
    {
        public object _lock = new object();

        public async Task<UIImage> AddGetUIImage(T key, string urlImage)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }
            else
            {
                UIImage image = await Utils.GetUIImageFromUrlAsync(urlImage);

                if (image == null)
                {
                    return null;
                }

                lock (_lock)
                {
                    if (!ContainsKey(key))
                    {
                        Add(key, image);
                    }
                }

                return image;
            }
        }
    }
}