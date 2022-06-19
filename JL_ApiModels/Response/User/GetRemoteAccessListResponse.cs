using JL_MSSQLServer.PersistModels;

namespace JL_ApiModels.Response.User
{
    public class GetRemoteAccessListResponse : ResponseBase, IResponse
    {
        public List<UserRemoteAccessWithUserData> UserRemoteAccesses { get; set; }
    }

    public class UserRemoteAccessWithUserData
    {
        public UserRemoteAccess UserRemote { get; set; }
        public string UserName { get; set; }
    }
}
