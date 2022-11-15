using File = HtmlConverter.Domain.Models.File;
using Microsoft.EntityFrameworkCore;

namespace HtmlConverter.Application.Interfaces
{
    public interface IBaseRepository<T> where T : File
    {
        DbSet<File> Files { get; }
        Task SaveChangesAsync();
    }
}
