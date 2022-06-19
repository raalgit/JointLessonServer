using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class FileDataRepository : RepositoryBase<FileData>, IFileDataRepository
    {
        public FileDataRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
