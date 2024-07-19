using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        public byte Level { get; set; }

        public long PersonId { get; set; }
    }
}