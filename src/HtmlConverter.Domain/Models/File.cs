using HtmlConverter.Domain.Models.Common;
using HtmlConverter.Domain.Models.enums;

namespace HtmlConverter.Domain.Models
{
    public class File: BaseEntity
    {
        public  string? ContentType { get; set; }

        public FileFormat FileFormat { get; set; } = FileFormat.UNKNOWN;
        
        public string? Name { get; set; }

        public byte[]? FileData { get; set; }
    }
}
