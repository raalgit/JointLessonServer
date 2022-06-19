using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class SignalUserConnectionRepository : RepositoryBase<SignalUserConnection>, ISignalUserConnectionRepository
    {
        public SignalUserConnectionRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
