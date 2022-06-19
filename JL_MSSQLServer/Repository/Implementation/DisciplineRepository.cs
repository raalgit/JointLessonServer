using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class DisciplineRepository : RepositoryBase<Discipline>, IDisciplineRepository
    {
        public DisciplineRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
