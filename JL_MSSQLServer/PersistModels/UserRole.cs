using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("UserRole", Schema = "JL")]
    public class UserRole : IPersist
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    }
}
