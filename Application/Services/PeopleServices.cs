using Application.Data;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PeopleServices : IPeopleServices
    {
        private readonly IPersonRepository _personRepository;

        public PeopleServices(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<List<Person>> GetAll()
        {
            return (await _personRepository.GetAll()).ToList();
        }

        public async Task<Person> GetById(long id)
        {
            return await _personRepository.GetById(id);
        }

        public async Task Create(Person person)
        {
            await _personRepository.Create(person);
        }

        public async Task UpdateById(Person person)
        {
            await _personRepository.UpdateById(person);
        }

        public async Task DeleteById(long id)
        {
            await _personRepository.DeleteById(id);
        }
    }
}


