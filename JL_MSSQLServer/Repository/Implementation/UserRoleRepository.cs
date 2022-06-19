using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class UserRoleRepository : RepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
