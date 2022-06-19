using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("AuthData", Schema = "JL")]
    public class AuthData : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
