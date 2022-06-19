using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("WorkBook", Schema = "JL")]
    public class WorkBook : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FileDataId { get; set; }
        public int CourseId { get; set; }
    }
}
