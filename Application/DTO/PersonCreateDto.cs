namespace Application.DTO
{
    public class PersonCreateDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public List<SkillCreateDto> Skills { get; set; } = new List<SkillCreateDto>();
    }
}
