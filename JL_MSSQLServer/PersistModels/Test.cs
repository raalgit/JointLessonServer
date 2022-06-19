using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Test", Schema = "JL")]
    public class Test : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public DateTime SendDate { get; set; }
        public string PageId { get; set; }
    }
}
