using Microsoft.AspNetCore.SignalR;

namespace JL_SignalR
{
    public class SignalRUserProvider : IUserIdProvider, ISignalRUserProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity.Name;
        }
    }
}
