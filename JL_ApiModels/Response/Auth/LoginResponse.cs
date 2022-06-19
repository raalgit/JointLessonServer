using JL_MSSQLServer.PersistModels;

namespace JL_ApiModels.Response.Auth
{
    public class LoginResponse : ResponseBase, IResponse
    {
        public JL_MSSQLServer.PersistModels.User User { get; set; }
        public List<Role> Roles { get; set; }
        public string JWT { get; set; }
    }
}
