using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ToText.API.Extensions
{
    public static class ImageExtensions
    {

        public static byte[] ToByteArray(this Image image)
        {
            //return (byte[])new ImageConverter().ConvertTo(image, typeof(byte[]));
            using (MemoryStream mStream = new MemoryStream())
            {
                image.Save(mStream, image.RawFormat);
                return mStream.ToArray();
            }
        }

        private static readonly Dictionary<Guid, string> _knownImageFormats =
            (from p in typeof(ImageFormat).GetProperties(BindingFlags.Static | BindingFlags.Public)
             where p.PropertyType == typeof(ImageFormat)
             let value = (ImageFormat)p.GetValue(null, null)
             select new { value.Guid, Name = value.ToString() })
            .ToDictionary(p => p.Guid, p => p.Name);

        public static string GetImageFormat(this Image image)
        {
            if (_knownImageFormats.TryGetValue(image.RawFormat.Guid, out string name))
                return name.ToLower();
            return null;

            //System.Drawing.Imaging.ImageFormat.
            //return new ImageFormatConverter().ConvertToString(image.RawFormat).ToLower();
            //return image.RawFormat.ToString().ToLower();
        }

    }
}
