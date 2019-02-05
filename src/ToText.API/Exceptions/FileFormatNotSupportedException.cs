using System;

namespace ToText.API.Exceptions
{
    [Serializable]
    public class FileFormatNotSupportedException : Exception
    {
        public File File { get; set; }

        public FileFormatNotSupportedException(File file)
            : base("File format not supported. supported formats (tif, jpg, bmp, png, pdf).")
        {
            File = file;
        }
    }
}
