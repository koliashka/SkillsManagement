using Application.DTO;

namespace Application.Interfaces
{
    public interface IPeopleServices
    {
        Task<List<PersonDto>> GetAllAsync();
        Task<PersonDto> GetByIdAsync(long id);
        Task<PersonDto> CreateAsync(PersonCreateDto personCreateDto);
        Task UpdateByIdAsync(long id, PersonUpdateDto personUpdateDto);
        Task DeleteByIdAsync(long id);
    }

}

