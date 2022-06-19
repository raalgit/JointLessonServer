using JL_ApiModels.Request.Auth;
using JL_ApiModels.Response.Auth;

namespace JL_Service.Abstraction.Auth
{
    public interface ILoginPoint: IPoint<LoginRequest, LoginResponse>
    {
    }
}
