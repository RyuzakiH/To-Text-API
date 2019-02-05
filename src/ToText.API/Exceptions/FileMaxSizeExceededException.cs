using System;

namespace ToText.API.Exceptions
{
    [Serializable]
    public class FileMaxSizeExceededException : Exception
    {
        public File File { get; set; }
        
        public FileMaxSizeExceededException(File file)
            : base("File size exceeded max file size (5 MB)")
        {
            File = file;
        }
    }
}
