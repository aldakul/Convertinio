using HtmlConverter.Persistence.Context;

namespace HtmlConverter.Persistence.Repositories
{
    public class FilesRepository : BaseRepository<Domain.Models.File>
    {
        public FilesRepository(FilesConverterDbContext filesConverterDbContext) 
            : base(filesConverterDbContext) { }
    }
}
