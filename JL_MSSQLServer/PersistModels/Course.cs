﻿using System.ComponentModel.DataAnnotations.Schema;

namespace JL_MSSQLServer.PersistModels
{
    [Table("Course", Schema = "JL")]
    public class Course : IPersist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AvatarId { get; set; }
        public int ManualId { get; set; }
        public int DisciplineId { get; set; }
    }
}
