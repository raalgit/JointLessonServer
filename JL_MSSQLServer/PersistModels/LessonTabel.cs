using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("LessonTabel", Schema = "JL")]
    public class LessonTabel : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public bool HandUp { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime? LeaveDate { get; set; }
    }
}
