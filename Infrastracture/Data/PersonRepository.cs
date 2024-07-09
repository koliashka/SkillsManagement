using Application.Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using SkillsManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _context.Persons.Include(p => p.Skills).ToListAsync();
        }

        public async Task<Person> GetById(long id)
        {
            return await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Create(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateById(Person person)
        {
            var existingPerson = await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == person.Id);
            if (existingPerson == null)
            {
                throw new InvalidOperationException($"Person with Id {person.Id} not found.");
            }

            existingPerson.Name = person.Name;
            existingPerson.DisplayName = person.DisplayName;

            _context.Skills.RemoveRange(existingPerson.Skills);
            existingPerson.Skills = person.Skills;

            // Обновление состояния сущности
            _context.Entry(existingPerson).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteById(long id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
    }
}

