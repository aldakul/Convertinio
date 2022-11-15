using HtmlConverter.Domain.Models.enums;

namespace HtmlConverter.Application.Common.Utils
{
    partial class FileExtensions
    {
        public static FileFormat GetFileFormat(string extension)
        {
            var fileType = extension.Replace(".", "").ToUpper();
            if (FileFormat.IsDefined(typeof(FileFormat), fileType))
                return (FileFormat)FileFormat.Parse(typeof(FileFormat), fileType);
            return FileFormat.UNKNOWN;
        }
    }
}
