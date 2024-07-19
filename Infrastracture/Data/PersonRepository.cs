using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SkillsManagement.Data;

namespace Infrastructure.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Persons.Include(p => p.Skills).ToListAsync();
        }

        public async Task<Person> GetByIdAsync(long id)
        {
            return await _context.Persons.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateByIdAsync(Person person)
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

            _context.Entry(existingPerson).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(long id)
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

