using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class ManualRepository : RepositoryBase<Manual>, IManualRepository
    {
        public ManualRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
