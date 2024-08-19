using Domain.Models;

namespace Application.Interfaces
{
    /// <summary>
    /// Интерфейс для управления объектами <see cref="Person"/> в репозитории.
    /// </summary>
    public interface IPersonRepository
    {
        /// <summary>
        /// Асинхронно получает всех людей из репозитория.
        /// </summary>
        /// <returns>Коллекция объектов <see cref="Person"/>.</returns>
        Task<IEnumerable<Person>> GetAllAsync();

        /// <summary>
        /// Асинхронно получает человека по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор человека.</param>
        /// <returns>Объект <see cref="Person"/>, если найден; иначе <see langword="null"/>.</returns>
        Task<Person> GetByIdAsync(long id);

        /// <summary>
        /// Асинхронно создает нового человека в репозитории.
        /// </summary>
        /// <param name="person">Объект <see cref="Person"/>, который нужно создать.</param>
        Task CreateAsync(Person person);

        /// <summary>
        /// Асинхронно обновляет данные человека по его идентификатору в репозитории.
        /// </summary>
        /// <param name="person">Объект <see cref="Person"/> с обновленными данными.</param>
        Task UpdateByIdAsync(Person person);

        /// <summary>
        /// Асинхронно удаляет человека из репозитория по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор человека.</param>
        Task DeleteByIdAsync(long id);
    }
}
