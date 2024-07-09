using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Skill
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        public byte Level { get; set; }

        public long PersonId { get; set; }
    }
}