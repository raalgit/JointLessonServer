using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class LessonTabelRepository : RepositoryBase<LessonTabel>, ILessonTabelRepository
    {
        public LessonTabelRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
