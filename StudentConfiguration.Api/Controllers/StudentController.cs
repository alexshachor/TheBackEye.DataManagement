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
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IStudentRepository _studentRepository;


        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StudentController>/5
        /// <summary>
        /// Get the StudentDto object by the student id nmumber
        /// </summary>
        /// <param name="birthId">The identity number (from birth) of the student</param>
        /// <returns>StudentDto object contains all of the student's personal details</returns>
        [HttpGet("{birthId}")]
        public async Task<ActionResult<StudentDto>> Get(int birthId)
        {
            //validate request
            if (birthId < 1)
            {
                string msg = $"birthId: {birthId} must be positive number";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get student from DB
                var student = await _studentRepository.GetStudentByBirthId(birthId);
                if (student == null)
                {
                    string msg = $"student with birth id: {birthId} not found in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(student.ToDto());
                }
            }
            catch(Exception e)
            {
                string msg = $"cannot get student with birth id: {birthId}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }

        }

        // POST api/<StudentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
