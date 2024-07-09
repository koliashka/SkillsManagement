using Application.Data;
using Application.Services;
using Azure.Core;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillsManagement.Data;
using System.Threading;


namespace SkillsManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPeopleServices _peopleServices;


        public PersonsController(ApplicationDbContext context, IPeopleServices peopleServices)
        {
            _peopleServices = peopleServices;
            _context = context;
        }

        // GET: api/persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            var persons = await _peopleServices.GetAll();
            return Ok(persons);
        }

        // GET: api/persons/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _peopleServices.GetById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);
        }

        // POST: api/persons
        [HttpPost]
        public async Task<ActionResult> CreatePerson([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest("Invalid data.");
            }

            await _peopleServices.Create(person);
            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // PUT: api/persons/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(long id, [FromBody] Person person)
        {

            // Вызываем сервис для обновления по идентификатору
            await _peopleServices.UpdateById(person);

            return NoContent();
        }

        private bool PersonExists(long id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }

        // DELETE: api/persons/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(long id)
        {
            var existingPerson = await _peopleServices.GetById(id);
            if (existingPerson == null)
            {
                return NotFound();
            }

            await _peopleServices.DeleteById(id);
            return NoContent();
        }
    }

}