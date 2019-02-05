using System;

namespace ToText.API.Exceptions
{
    [Serializable]
    public class FileConversionErrorException : Exception
    {
        public File File { get; set; }

        public FileConversionErrorException(File file)
            : base("File could not be converted to text.")
        {
            File = file;
        }
    }
}
