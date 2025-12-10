using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.Threading.Tasks;
using CoreGraphics;
using System.Drawing;
using Palmipedo.iOS.Core;

namespace Palmipedo.iOS
{
    public static class Utils
    {
        public static UIImage GetUIImageFromUrl(this UIImage image, string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }

        public static UIImage GetUIImageFromUrl(string uri)
        {
            //return null;
            try
            {
                if (string.IsNullOrEmpty(uri)) return null;

                uri = uri.Replace(" ", "%20");
                //È
                using (var url = new NSUrl(uri))
                {
                    if (url == null) return null;

                    using (var data = NSData.FromUrl(url))
                    {
                        if (data == null) return null;

                        return UIImage.LoadFromData(data);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Task<UIImage> GetUIImageFromUrlAsync(string uri)
        {
            //return Task.FromResult<UIImage>(null);
            return Task<UIImage>.Factory.StartNew(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(uri)) return null;

                    uri = uri.Replace(" ", "%20");
                    //uri = uri.Replace("-", "%2D");
                    //uri = uri.Replace("."%2E
                    //È
                    using (var url = new NSUrl(uri))
                    {
                        if (url == null) return null;

                        using (var data = NSData.FromUrl(url))
                        {
                            if (data == null) return null;

                            return UIImage.LoadFromData(data);
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public static Task<NSString> GetStringFromUrlAsync(string uri)
        {
            return Task<NSString>.Factory.StartNew(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(uri)) return null;

                    uri = uri.Replace(" ", "%20");
                    //È
                    using (var url = new NSUrl(uri))
                    {
                        if (url == null) return null;

                        using (var data = NSData.FromUrl(url))
                        {
                            if (data == null) return null;

                            NSStringEncoding encoding;
                            if (Context.CurrentLang == Core.Entities.Common.LANGUAGE_RUSSO)
                            {
                                encoding = NSStringEncoding.UTF8;
                            }
                            else
                            {
                                encoding = NSStringEncoding.WindowsCP1252;
                            }

                            return NSString.FromData(data, encoding);
                        }
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        public static UIImage ResizeImage(this UIImage sourceImage, float width, float height)
        {
            UIGraphics.BeginImageContextWithOptions(new System.Drawing.SizeF(width, height), false, 0.4f);

            CGContext context = UIGraphics.GetCurrentContext();
            context.InterpolationQuality = CGInterpolationQuality.High;

            context.TranslateCTM(0, height);
            context.ScaleCTM(1f, -1f);

            context.DrawImage(new RectangleF(0, 0, width, height), sourceImage.CGImage);

            var scaledImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return UIImage.LoadFromData(scaledImage.AsPNG());


            //sourceImage.Draw(new System.Drawing.RectangleF(0, 0, width, height));
            //var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            //UIGraphics.EndImageContext();
            //return resultImage;
        }

        public static DateTime ToDateTime(this NSDate date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
            return reference.AddSeconds(date.SecondsSinceReferenceDate);
        }

        public static NSDate ToNSDate(this DateTime date)
        {
            DateTime reference = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate((date - reference).TotalSeconds);
        }

        public static byte[] ToJpgBuffer(this UIImage sourceImage)
        {
            using (NSData imageData = sourceImage.AsJPEG())
            {
                byte[] myByteArray = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
                return myByteArray;
            }
        }

        public static UIButton GetCancelButton(this UISearchBar searchBar)
        {
            //Look for a button, probably only one button, and that is probably the cancel button.
            return searchBar.GetAllSubViews().OfType<UIButton>().FirstOrDefault();
        }

        /// <summary>
        /// Recursively traverses all subviews and returns them in a little list.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static IEnumerable<UIView> GetAllSubViews(this UIView view)
        {
            List<UIView> retList = new List<UIView>();
            retList.AddRange(view.Subviews);
            foreach (var subview in view.Subviews)
            {
                retList.AddRange(subview.GetAllSubViews());
            }

            return retList;
        }
    }

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return input.First().ToString().ToUpper() + string.Join("", input.Skip(1)).ToLower();
        }

        public static nfloat GetUIViewWithFromString(this string value, UIFont font)
        {
            if (string.IsNullOrEmpty(value))
                return 0f;

            UILabel uiLabel = new UILabel();
            uiLabel.Text = value;
            uiLabel.Font = font;
            CGSize length = uiLabel.Text.StringSize(uiLabel.Font);
            return length.Width;// + _paddingLeftRightButton * 2; //- ((length.Width / 100 ) * 10); // mi sembra che ci sia un aumento del 10%
        }
        //public static CGSize GetUIViewSizeFromString(this string value, UIFont font, float maxWidth)
        //{
        //    UILabel uiLabel = new UILabel();
        //    uiLabel.Text = value;
        //    uiLabel.Font = font;
        //    uiLabel.Lines = 0;
        //    uiLabel.LineBreakMode = UILineBreakMode.WordWrap;
        //    CGSize length = uiLabel.Text.StringSize(uiLabel.Font);
        //    return length;
        //}
    }


}