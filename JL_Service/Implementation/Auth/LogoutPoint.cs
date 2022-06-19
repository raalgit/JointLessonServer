using JL_ApiModels.Response.Auth;
using JL_Service.Abstraction.Auth;
using JL_Utility.Models;

namespace JL_Service.Implementation.Auth
{
    public class LogoutPoint : ILogoutPoint
    {
        public async Task<LogoutResponse> Execute(object? req, UserSettings userSettings)
        {
            throw new NotImplementedException();
        }

        public Task<LogoutResponse> Start(object? req, UserSettings userSettings)
        {
            throw new NotImplementedException();
        }
    }
}
