using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("UserRemoteAccess", Schema = "JL")]
    public class UserRemoteAccess : IPersist
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public string ConnectionData { get; set; }
        public DateTime StartDate { get; set; }
    }
}
