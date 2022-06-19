using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Group", Schema = "JL")]
    public class Group : IPersist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
