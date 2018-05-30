using System;
using System.Drawing;
using System.IO;

namespace ToText
{
    public class File
    {
        private const string imageType = "image/{0}";
        private const string pdfType = "application/pdf";


        public byte[] Bytes { get; private set; }
        public string Type { get; private set; }
        public string Format { get; private set; }
        public string Name { get; private set; }


        public File(string path = null)
        {
            if (path != null)
                Load(path);
        }


        public void Load(string path)
        {
            Init(Utilities.ReadFileBytes(path), Path.GetFileName(path));
        }

        public void Load(Image image)
        {
            Init(Utilities.ImageToByteArray(image));
        }

        public void Load(byte[] bytes)
        {
            Init(bytes);
        }


        private void Init(byte[] bytes, string name = null)
        {
            this.Bytes = bytes;

            this.Format = GetFileFormat();
            this.Type = GetFileType();

            this.Name = name ?? GenerateRandomFileName(this.Format);
        }


        private string GetFileFormat()
        {
            if (IsValidImage(this.Bytes))
                return Utilities.GetImageFormat(Utilities.ByteArrayToImage(this.Bytes));
            else if (IsPdf(this.Bytes))
                return "pdf";
            return null;
        }

        private string GetFileType()
        {
            if (this.Format == null)
                return null;

            if (this.Format == "pdf")
                return pdfType;
            else
                return string.Format(imageType, this.Format);
        }


        private static bool IsValidImage(byte[] bytes)
        {
            try { Image.FromStream(new MemoryStream(bytes)); }
            catch (ArgumentException) { return false; }
            return true;
        }

        private static bool IsPdf(byte[] bytes)
        {
            return (bytes[0] == 0x25 && bytes[1] == 0x50 && bytes[2] == 0x44 && bytes[3] == 0x46);
        }


        private static string GenerateRandomFileName(string ext)
        {
            return string.Format("file{0}.{1}", new Random().Next(9), ext);
        }

    }
}
