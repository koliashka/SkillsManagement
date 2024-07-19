using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SkillsManagement.Data;
using Application.DTO;

namespace SkillsManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPeopleServices _peopleServices;
        private readonly ILogger<PersonsController> _logger;

        // Constructor to inject dependencies
        public PersonsController(ApplicationDbContext context, IPeopleServices peopleServices, ILogger<PersonsController> logger)
        {
            _peopleServices = peopleServices;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all persons.
        /// </summary>
        /// <returns>A list of all persons.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersonsAsync()
        {
            try
            {
                var persons = await _peopleServices.GetAllAsync();
                return Ok(persons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving persons.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person.</param>
        /// <returns>The person with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPersonAsync(long id)
        {
            try
            {
                var person = await _peopleServices.GetByIdAsync(id);
                if (person == null)
                {
                    return NotFound();
                }
                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving person with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new person.
        /// </summary>
        /// <param name="personCreateDto">The data transfer object containing person creation data.</param>
        /// <returns>The created person.</returns>
        [HttpPost]
        public async Task<ActionResult> CreatePersonAsync([FromBody] PersonCreateDto personCreateDto)
        {
            try
            {
                if (personCreateDto == null)
                {
                    return BadRequest("Invalid data.");
                }

                var person = new Person
                {
                    Name = personCreateDto.Name,
                    DisplayName = personCreateDto.DisplayName,
                    Skills = personCreateDto.Skills.Select(s => new Skill
                    {
                        Name = s.Name,
                        Level = s.Level
                    }).ToList()
                };

                await _peopleServices.CreateAsync(person);
                return Ok(person);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating person.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to update.</param>
        /// <param name="personUpdateDto">The data transfer object containing person update data.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonAsync(long id, [FromBody] PersonUpdateDto personUpdateDto)
        {
            try
            {
                if (personUpdateDto == null)
                {
                    return BadRequest("Invalid data.");
                }

                var person = new Person
                {
                    Id = id, // Set the ID from the URL parameter
                    Name = personUpdateDto.Name,
                    DisplayName = personUpdateDto.DisplayName,
                    Skills = personUpdateDto.Skills.Select(s => new Skill
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Level = s.Level
                    }).ToList()
                };

                await _peopleServices.UpdateByIdAsync(person);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating person with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to delete.</param>
        /// <returns>A status indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonAsync(long id)
        {
            try
            {
                await _peopleServices.DeleteByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting person with id {id}.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
