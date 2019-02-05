using System.Drawing;
using System.IO;

namespace ToText.API.Extensions
{
    public static class Extensions
    {        
        public static Image ToImage(this byte[] imageBytes)
        {
            //return (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
            using (var ms = new MemoryStream(imageBytes))
                return new Bitmap(ms);
        }

    }
}
