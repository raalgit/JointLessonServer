using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Manual", Schema = "JL")]
    public class Manual : IPersist
    {
        public int Id { get; set; }
        public int FileDataId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
    }
}
