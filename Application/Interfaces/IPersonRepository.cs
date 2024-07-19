using Domain.Models;

namespace Application.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(long id);
        Task CreateAsync(Person person);
        Task UpdateByIdAsync(Person person);
        Task DeleteByIdAsync(long id);

    }
}
