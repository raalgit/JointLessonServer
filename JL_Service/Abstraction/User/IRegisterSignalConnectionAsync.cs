using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;

namespace JL_Service.Abstraction.User
{
    public interface IRegisterSignalConnectionAsync : IPoint<string, RegisterSignalConnectionResponse>
    {
    }
}
