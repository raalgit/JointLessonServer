using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("UserGroup", Schema = "JL")]
    public class UserGroup : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}
