using Microsoft.EntityFrameworkCore;
using File = HtmlConverter.Domain.Models.File;

namespace HtmlConverter.Persistence.Context
{
    public class FilesConverterDbContext : DbContext
    {
        public FilesConverterDbContext(DbContextOptions<FilesConverterDbContext> options)
            : base(options) { }
        public DbSet<File> Files { get; set; }
    }
}
