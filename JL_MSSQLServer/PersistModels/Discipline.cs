using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Discipline", Schema = "JL")]
    public class Discipline : IPersist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
