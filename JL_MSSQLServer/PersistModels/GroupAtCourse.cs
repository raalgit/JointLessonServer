using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("GroupAtCourse", Schema = "JL")]
    public class GroupAtCourse : IPersist
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
