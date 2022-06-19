using JL_MSSQLServer.PersistModels;

namespace JL_Utility.Models
{
    public class UserSettings
    {
        public User User { get; set; }

        public UserSettings(User user)
        {
            User = user;
        }
    }
}
