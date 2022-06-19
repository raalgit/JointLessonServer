using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class AuthDataRepository : RepositoryBase<AuthData>, IAuthDataRepository
    {
        public AuthDataRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
