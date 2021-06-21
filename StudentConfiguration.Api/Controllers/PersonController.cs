using DbAccess.RepositoryInterfaces;
using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentConfiguration.Api.Controllers
{
    /// <summary>
    /// PersonController is responsible for all the Person's CRUD operations using API calls 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IPersonRepository _personRepository;


        public PersonController(ILogger<PersonController> logger, IPersonRepository personRepository)
        {
            _logger = logger;
            _personRepository = personRepository;
        }

        /// <summary>
        /// Get PersonDto object by the person password
        /// </summary>
        /// <param name="password">The password of the person (student)</param>
        /// <response code="200">PersonDto object contains all of the person's personal details</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the person in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(PersonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpGet("{password}")]
        public async Task<ActionResult<PersonDto>> Get(string password)
        {
            //validate request
            if (String.IsNullOrWhiteSpace(password))
            {
                string msg = $"password: {password} must not be null or empty";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get person from DB
                var person = await _personRepository.GetPersonByPassword(password);
                if (person == null)
                {
                    string msg = $"person with password: {password} not found in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(person.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get person with password: {password}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }

        }

        /// <summary>
        /// Add a new person to DB
        /// </summary>
        /// <param name="personDto">PersonDto object contains all of the person's personal details which will be added to DB</param>
        /// <response code="200">PersonDto object contains all of the person's personal details from DB</response>
        /// <response code="400">BadRequest - invalid values (Person is null)</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPost]
        [ProducesResponseType(typeof(PersonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PersonDto>> Post([FromBody] PersonDto personDto)
        {
            //TODO: add validation for each filed
            //validate request
            if (personDto == null)
            {
                string msg = $"personDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //add person to DB
                var person = await _personRepository.AddPerson(personDto.ToModel());
                if (person == null)
                {
                    string msg = $"cannot add person to DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(person.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot add person to DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Change Person in the DB
        /// </summary>
        /// <param name="personDto">PersonDto object contains all of the new person's details</param>
        /// <response code="200">PersonDto object contains all of the details from DB</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPut]
        [ProducesResponseType(typeof(PersonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PersonDto>> Put([FromBody] PersonDto personDto)
        {
            //TODO: add validation for each filed
            //validate request
            if (personDto == null)
            {
                string msg = $"personDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //change person DB
                var person = await _personRepository.UpdatePerson(personDto.ToModel());
                if (person == null)
                {
                    string msg = $"cannot change the person with person id: {personDto.Id}";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(person.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot change the person with person id: {personDto.Id}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
    }
}
