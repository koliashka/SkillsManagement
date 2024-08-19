using Application.Interfaces;
using Domain.Models;
using Infrastracture.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    /// <summary>
    /// Репозиторий для управления объектами <see cref="Person"/>.
    /// </summary>
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PersonRepository"/> с указанным контекстом базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Асинхронно получает всех людей из базы данных вместе с их навыками.
        /// </summary>
        /// <returns>Коллекция объектов <see cref="Person"/>.</returns>
        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _context.Persons.AsNoTracking().Include(p => p.Skills).ToListAsync();
        }

        /// <summary>
        /// Асинхронно получает человека по его идентификатору вместе с его навыками.
        /// </summary>
        /// <param name="id">Идентификатор человека.</param>
        /// <returns>Объект <see cref="Person"/>, если найден; иначе <see langword="null"/>.</returns>
        public async Task<Person> GetByIdAsync(long id)
        {
            return await _context.Persons.AsNoTracking().Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <summary>
        /// Асинхронно создает нового человека в базе данных.
        /// </summary>
        /// <param name="person">Объект <see cref="Person"/>, который нужно создать.</param>
        public async Task CreateAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Асинхронно обновляет данные человека по его идентификатору.
        /// </summary>
        /// <param name="person">Объект <see cref="Person"/> с обновленными данными.</param>
        /// <exception cref="InvalidOperationException">Выбрасывается, если человек с указанным идентификатором не найден.</exception>
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

        /// <summary>
        /// Асинхронно удаляет человека из базы данных по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор человека.</param>
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
