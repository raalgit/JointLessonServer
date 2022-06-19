using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Role", Schema = "JL")]
    public class Role : IPersist
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
    }
}
