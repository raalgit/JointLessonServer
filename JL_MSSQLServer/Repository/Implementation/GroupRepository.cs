using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
