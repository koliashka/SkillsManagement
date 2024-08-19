namespace Application.DTO
{
    public class PersonUpdateDto
    {
        public string Name { get; set; }   = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<SkillUpdateDto> Skills { get; set; } = new List<SkillUpdateDto>();
    }
}
