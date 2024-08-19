using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Skill
    {
        [Key]
       
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        public byte Level { get; set; }

        [Key]
        [Column(Order = 0)]
        public long PersonId { get; set; }
    }
}
