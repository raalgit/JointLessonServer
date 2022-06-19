using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class WorkBookRepository : RepositoryBase<WorkBook>, IWorkBookRepository
    {
        public WorkBookRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
