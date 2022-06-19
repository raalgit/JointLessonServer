using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class GroupAtCourseRepository : RepositoryBase<GroupAtCourse>, IGroupAtCourseRepository
    {
        public GroupAtCourseRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
