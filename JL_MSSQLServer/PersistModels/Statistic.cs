using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Statistic", Schema = "JL")]
    public class Statistic : IPersist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SuccessExecution { get; set; }
        public string FailedExecution { get; set; }
    }
}
