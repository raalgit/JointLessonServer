using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("FileData", Schema = "JL")]
    public class FileData : IPersist
    {
        public int Id { get; set; }
        public string MongoName { get; set; }
        public string OriginalName { get; set; }
        public string MongoId { get; set; }
    }
}
