using System;
using System.Drawing;
using System.IO;
using ToText.API.Extensions;

namespace ToText.API
{
    public class File
    {
        private const string imageType = "image/{0}";
        private const string pdfType = "application/pdf";


        public byte[] Bytes { get; private set; }
        public string Type { get; private set; }
        public string Format { get; private set; }
        public string Name { get; private set; }
        public long Size { get; private set; }

        public File()
        {

        }

        public File(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                Load(path);
        }

        public File(Image image)
        {
            if (image != null)
                Load(image);
        }

        public File(byte[] bytes)
        {
            if (bytes != null)
                Load(bytes);
        }


        public void Load(string path)
        {
            var fileBytes = System.IO.File.ReadAllBytes(path);
            Init(fileBytes, Path.GetFileName(path));
        }

        public void Load(Image image)
        {
            Init(image.ToByteArray());
        }

        public void Load(byte[] bytes)
        {
            Init(bytes);
        }


        private void Init(byte[] bytes, string name = null)
        {
            Bytes = bytes;

            Format = GetFileFormat().Replace("jpeg", "jpg");
            Type = GetFileType();

            Name = name ?? GenerateRandomFileName(this.Format);

            Size = bytes.Length;
        }


        private string GetFileFormat()
        {
            if (IsValidImage(Bytes))
                return Bytes.ToImage().GetImageFormat();
            else if (IsPdf(Bytes))
                return "pdf";
            return null;
        }

        private string GetFileType()
        {
            if (Format == null)
                return null;

            if (Format == "pdf")
                return pdfType;
            else
                return string.Format(imageType, Format);
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
