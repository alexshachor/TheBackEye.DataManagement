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
    /// TeacherController is responsible for all the teacher's CRUD operations using API calls 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITeacherRepository _teacherRepository;


        public TeacherController(ILogger<TeacherController> logger, ITeacherRepository teacherRepository)
        {
            _logger = logger;
            _teacherRepository = teacherRepository;
        }

        /// <summary>
        /// Get TeacherDto object by the teacher password
        /// </summary>
        /// <param name="password">The password of the teacher</param>
        /// <response code="200">TeacherDto object contains all of the teacher's personal details</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the teacher in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(TeacherDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpGet("{password}")]
        public async Task<ActionResult<TeacherDto>> Get(string password)
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
                //get student from DB
                var teacher = await _teacherRepository.GetTeacherByPassword(password);
                if (teacher == null)
                {
                    string msg = $"teacher with password: {password} not found in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(teacher.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get teacher with password: {password}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }

        }

        /// <summary>
        /// Add a new teacher to DB
        /// </summary>
        /// <param name="teacherDto">TeachertDto object contains all of the teacher's personal details which will be added to DB</param>
        /// <response code="200">TeacherDto object contains all of the teacher's personal details from DB</response>
        /// <response code="400">BadRequest - invalid values (Teacher or Person is null)</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPost]
        [ProducesResponseType(typeof(TeacherDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TeacherDto>> Post([FromBody] TeacherDto teacherDto)
        {
            //TODO: add validation for each filed
            //validate request
            if (teacherDto == null || teacherDto.Person == null)
            {
                string msg = $"teacherDto or personDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //add student to DB
                var teacher = await _teacherRepository.AddTeacher(teacherDto.ToModel());
                if (teacher == null)
                {
                    string msg = $"cannot add teacher with password: {teacherDto.Password} to DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(teacher.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot add teacher with password: {teacherDto.Password} to DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Delete TeacherDto object by the teacher password
        /// </summary>
        /// <param name="password">The password of the teacher</param>
        /// <response code="200">bool true</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the teacher in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpDelete("{password}")]
        public async Task<ActionResult<bool>> Delete(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                string msg = $"password is null or empty";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //remove teacher from DB
                var teacher = await _teacherRepository.RemoveTeacherByPassword(password);
                if (teacher == null)
                {
                    string msg = $"cannot remove the teacher with the password: {password} from DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(teacher);
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot remove the teacher with the password: {password} from DB. Due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
    }
}