using Application.DTO;
using Application.Interfaces;
using Application.Exceptions;
using Domain.Models;

namespace Application.Services
{
    /// <summary>
    /// Сервис для управления объектами <see cref="Person"/>.
    /// </summary>
    public class PeopleServices : IPeopleServices
    {
        private readonly IPersonRepository _personRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PeopleServices"/> с указанным репозиторием людей.
        /// </summary>
        /// <param name="personRepository">Репозиторий для управления объектами <see cref="Person"/>.</param>
        public PeopleServices(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        /// <summary>
        /// Асинхронно получает список всех людей.
        /// </summary>
        /// <returns>Список объектов <see cref="PersonDto"/>.</returns>
        public async Task<List<PersonDto>> GetAllAsync()
        {
            try
            {
                var persons = await _personRepository.GetAllAsync();
                return persons.Select(person => new PersonDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    DisplayName = person.DisplayName,
                    Skills = person.Skills.Select(skill => new SkillDto
                    {
                        Name = skill.Name,
                        Level = skill.Level
                    }).ToList()
                }).ToList();
            }
            catch (Exception)
            {
                throw new PersonOperationException("Error retrieving all persons.");
            }
        }

        /// <summary>
        /// Асинхронно получает человека по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор человека.</param>
        /// <returns>Объект <see cref="PersonDto"/>, если найден; иначе выбрасывается исключение <see cref="PersonNotFoundException"/>.</returns>
        public async Task<PersonDto> GetByIdAsync(long id)
        {
            try
            {
                var person = await _personRepository.GetByIdAsync(id);
                if (person == null)
                {
                    throw new PersonNotFoundException(id);
                }

                return new PersonDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    DisplayName = person.DisplayName,
                    Skills = person.Skills.Select(skill => new SkillDto
                    {
                        Name = skill.Name,
                        Level = skill.Level
                    }).ToList()
                };
            }
            catch (PersonNotFoundException)
            {
                throw; // Re-throw custom exception
            }
            catch (Exception)
            {
                throw new PersonOperationException($"Error retrieving person with ID {id}.");
            }
        }

        /// <summary>
        /// Асинхронно создает нового человека.
        /// </summary>
        /// <param name="personCreateDto">DTO для создания нового человека.</param>
        /// <returns>Созданный объект <see cref="PersonDto"/>.</returns>
        public async Task<PersonDto> CreateAsync(PersonCreateDto personCreateDto)
        {
            try
            {
                if (personCreateDto == null || personCreateDto.Skills.Any(skill => skill.Level < 1 || skill.Level > 10))
                {
                    throw new InvalidPersonDataException("Invalid person data or skill levels. Level must be 1-10");
                }

                var person = new Person
                {
                    Name = personCreateDto.Name,
                    DisplayName = personCreateDto.DisplayName,
                    Skills = personCreateDto.Skills.Select(skillDto => new Skill
                    {
                        Name = skillDto.Name,
                        Level = skillDto.Level
                    }).ToList()
                };

                await _personRepository.CreateAsync(person);

                // Return created DTO
                return new PersonDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    DisplayName = person.DisplayName,
                    Skills = person.Skills.Select(skill => new SkillDto
                    {
                        Name = skill.Name,
                        Level = skill.Level
                    }).ToList()
                };
            }
            catch (InvalidPersonDataException)
            {
                throw; // Re-throw custom exception
            }
            catch (Exception)
            {
                throw new PersonOperationException("Error creating person.");
            }
        }

        /// <summary>
        /// Асинхронно обновляет данные человека по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор человека.</param>
        /// <param name="personUpdateDto">DTO с обновленными данными.</param>
        public async Task UpdateByIdAsync(long id, PersonUpdateDto personUpdateDto)
        {
            
                if (personUpdateDto == null || personUpdateDto.Skills.Any(skill => skill.Level < 1 || skill.Level > 10))
                {
                    throw new InvalidPersonDataException("Invalid person data or skill levels.");
                }

                var existingPerson = await _personRepository.GetByIdAsync(id);
                if (existingPerson == null)
                {
                    throw new PersonNotFoundException(id);
                }

                existingPerson.Name = personUpdateDto.Name;
                existingPerson.DisplayName = personUpdateDto.DisplayName;
                existingPerson.Skills = personUpdateDto.Skills.Select(skillDto => new Skill
                {
                    Name = skillDto.Name,
                    Level = skillDto.Level
                }).ToList();

                await _personRepository.UpdateByIdAsync(existingPerson);
            
        }

        /// <summary>
        /// Асинхронно удаляет человека по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор человека.</param>
        public async Task DeleteByIdAsync(long id)
        {
            try
            {
                var person = await _personRepository.GetByIdAsync(id);
                if (person == null)
                {
                    throw new PersonNotFoundException(id);
                }

                await _personRepository.DeleteByIdAsync(id);
            }
            catch (PersonNotFoundException)
            {
                throw; // Re-throw custom exception
            }
            catch (Exception)
            {
                throw new PersonOperationException($"Error deleting person with ID {id}.");
            }
        }
    }
}
