using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class CourseTeacherRepository : RepositoryBase<CourseTeacher>, ICourseTeacherRepository
    {
        public CourseTeacherRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
