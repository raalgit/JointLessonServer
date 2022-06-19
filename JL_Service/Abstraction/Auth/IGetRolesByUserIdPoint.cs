using JL_MSSQLServer.PersistModels;

namespace JL_Service.Abstraction.Auth
{
    public interface IGetRolesByUserIdPoint : IPoint<int, List<Role>>
    {
    }
}
