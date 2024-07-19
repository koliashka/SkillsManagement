using Domain.Models;

namespace Application.Interfaces
{
    public interface IPeopleServices
    {
        /// <summary>
        /// Retrieves a list of all persons.
        /// </summary>
        /// <returns>A list of all persons.</returns>
        Task<List<Person>> GetAllAsync();
        /// <summary>
        /// Retrieves a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person.</param>
        /// <returns>The person with the specified ID.</returns>
        Task<Person> GetByIdAsync(long id);
        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="person">The person to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAsync(Person person);
        /// <summary>
        /// Updates an existing person by ID.
        /// </summary>
        /// <param name="person">The person to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateByIdAsync(Person person);
        /// <summary>
        /// Deletes a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteByIdAsync(long id);
    }
}

