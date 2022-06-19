using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Lesson", Schema = "JL")]
    public class Lesson : IPersist
    {
        public int Id { get; set; }
        public int? GroupAtCourseId { get; set; }
        public string? LastMaterialPage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TeacherId { get; set; }
        public string Type { get; set; }
        public int CourseId { get; set; }
    }
}
