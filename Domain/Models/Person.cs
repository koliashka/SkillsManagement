using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Person
    {
        public long Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "varchar(100)")]
        public string DisplayName { get; set; } = string.Empty;

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}