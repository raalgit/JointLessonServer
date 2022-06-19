using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class UserRemoteAccessRepository : RepositoryBase<UserRemoteAccess>, IUserRemoteAccessRepository
    {
        public UserRemoteAccessRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
