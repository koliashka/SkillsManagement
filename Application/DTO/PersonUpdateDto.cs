using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class PersonUpdateDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<SkillUpdateDto> Skills { get; set; } = new List<SkillUpdateDto>();
    }
}
