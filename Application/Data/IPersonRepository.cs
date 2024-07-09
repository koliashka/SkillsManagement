using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application.Data
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAll();
        Task<Person> GetById(long id);
        Task Create(Person person);
        Task UpdateById(Person person);
        Task DeleteById(long id);

    }
}
