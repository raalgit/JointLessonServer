using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_MSSQLServer.Repository.Implementation
{
    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
