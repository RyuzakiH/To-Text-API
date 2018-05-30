using System.Drawing;

namespace ToText
{
    public static class Utilities
    {

        public static byte[] ReadFileBytes(string path)
        {
            return System.IO.File.ReadAllBytes(path);
        }

        public static Image ByteArrayToImage(byte[] imageBytes)
        {
            return (Bitmap)((new ImageConverter()).ConvertFrom(imageBytes));
        }

        public static byte[] ImageToByteArray(Image image)
        {
            return (byte[])new ImageConverter().ConvertTo(image, typeof(byte[]));
        }

        public static string GetImageFormat(Image image)
        {
            return new ImageFormatConverter().ConvertToString(image.RawFormat).ToLower();
        }

    }
}
