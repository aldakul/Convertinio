using File = HtmlConverter.Domain.Models.File;
using HtmlConverter.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using HtmlConverter.Persistence.Context;

namespace HtmlConverter.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : File
    {
        private readonly FilesConverterDbContext _filesConverterDbContext;

        public BaseRepository(FilesConverterDbContext filesConverterDbContext)
        {
            _filesConverterDbContext = filesConverterDbContext;
        }

        public async virtual Task<IList<T>> GetAllAsync()
        {
            var result = await _filesConverterDbContext.Set<T>().Select(a => a).OrderByDescending(x => x.Created).ToListAsync();
            return result;
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            var result = await _filesConverterDbContext.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id);

            if (result == null)
                throw new NullReferenceException();
            return result;
        }

        public DbSet<File> Files => _filesConverterDbContext.Files;

        public async virtual Task SaveChangesAsync()
        {
            await _filesConverterDbContext.SaveChangesAsync();
        }
    }
}
