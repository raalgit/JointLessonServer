using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("CourseTeacher", Schema = "JL")]
    public class CourseTeacher : IPersist
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        public bool OnLesson { get; set; }
    }
}
