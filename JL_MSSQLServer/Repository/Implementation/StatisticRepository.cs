using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class StatisticRepository : RepositoryBase<Statistic>, IStatisticRepository
    {
        public StatisticRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
