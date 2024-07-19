using Domain.Models;

namespace Application.Interfaces
{
    public class PeopleServices : IPeopleServices
    {
        private readonly IPersonRepository _personRepository;

        public PeopleServices(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<List<Person>> GetAllAsync()
        {
            var persons = await _personRepository.GetAllAsync();
            return persons.ToList();
        }

       
        public async Task<Person> GetByIdAsync(long id)
        {
            return await _personRepository.GetByIdAsync(id);
        }

        
        public async Task CreateAsync(Person person)
        {
            await _personRepository.CreateAsync(person);
        }

       
        public async Task UpdateByIdAsync(Person person)
        {
            await _personRepository.UpdateByIdAsync(person);
        }

        
        public async Task DeleteByIdAsync(long id)
        {
            await _personRepository.DeleteByIdAsync(id);
        }
    }
}


