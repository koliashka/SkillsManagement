using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTO;

namespace SkillsManagement.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly IPeopleServices _peopleServices;
        private readonly ILogger<PersonsController> _logger;

        // Constructor to inject dependencies
        public PersonsController(IPeopleServices peopleServices, ILogger<PersonsController> logger)
        {
            _peopleServices = peopleServices;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersonsAsync()
        {
            var persons = await _peopleServices.GetAllAsync();
            return Ok(persons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPersonAsync(long id)
        {
            var person = await _peopleServices.GetByIdAsync(id);
            return Ok(person);
        }

        [HttpPost]
        public async Task<ActionResult<PersonDto>> CreatePersonAsync([FromBody] PersonCreateDto personCreateDto)
        {
            var person = await _peopleServices.CreateAsync(personCreateDto);
            return CreatedAtAction(nameof(GetPersonAsync), new { id = person.Id }, person);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonAsync(long id, [FromBody] PersonUpdateDto personUpdateDto)
        {
            await _peopleServices.UpdateByIdAsync(id, personUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonAsync(long id)
        {
            await _peopleServices.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}
