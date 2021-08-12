using DbAccess.RepositoryInterfaces;
using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DataManagement.Api.Controllers
{
    /// <summary>
    /// StudentLessonController is responsible for all the student leasson's CRUD operations using API calls 
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentLessonController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IStudentLessonRepository _studentLessonRepository;

        public StudentLessonController(ILogger<StudentLessonController> logger, IStudentLessonRepository studentLessonRepository)
        {
            _logger = logger;
            _studentLessonRepository = studentLessonRepository;
        }

        /// <summary>
        /// Get all the students participating in a given lesson id
        /// </summary>
        /// <param name="lessonId">Id of the lesson</param>
        /// <response code="200">List of personDto object (contains all of the details from DB) which are in the lesson</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpGet("{lessonId}")]
        [ProducesResponseType(typeof(List<PersonDto>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<PersonDto>>> Get(int lessonId)
        {
            //validate request
            if (lessonId < 0)
            {
                string msg = $"lessonId is invalid: {lessonId}";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get studnts in lesson from DB
                var students = await _studentLessonRepository.GetStudentsByLessonId(lessonId);
                if (students == null)
                {
                    string msg = $"cannot get students of lesson with id: {lessonId} from DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(students.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get student of lesson with id: {lessonId} to DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Add a new lesson to DB
        /// </summary>
        /// <param name="studentLessonDto">StudentLessonDto object contains all of the student lesson's details which will be added to DB</param>
        /// <response code="200">StudentLessonDto object contains all of the details from DB</response>
        /// <response code="400">BadRequest - null object or invalid values</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPost]
        [ProducesResponseType(typeof(StudentLessonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<StudentLessonDto>> Post([FromBody] StudentLessonDto studentLessonDto)
        {
            //validate request
            if (studentLessonDto == null)
            {
                string msg = $"studentLessonDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            if (studentLessonDto.LessonId < 0 || studentLessonDto.PersonId < 0)
            {
                string msg = $"lesson id: {studentLessonDto.LessonId} is invalid or" +
                    $" student id: {studentLessonDto.LessonId} is invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //add student lesson to DB
                var studentLesson = await _studentLessonRepository.AddStudentLesson(studentLessonDto.ToModel());
                if (studentLesson == null)
                {
                    string msg = $"cannot add student lesson with id: {studentLessonDto.Id} to DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(studentLesson.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot add student lesson with id: {studentLessonDto.Id} to DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
        /// <summary>
        /// Delete StudentLessonDto object by studentLessonId
        /// </summary>
        /// <param name="studentLessonId">The student Lesson relation we want to delete </param>
        /// <response code="200">bool true</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the student Lesson in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpDelete("{studentLessonId}")]
        public async Task<ActionResult<bool>> Delete(int studentLessonId)
        {
            //validate request
            if (studentLessonId < 0)
            {
                string msg = $"student lesson id: {studentLessonId} is invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //remove student Lesson from DB
                var result = await _studentLessonRepository.DeleteStudentLesson(studentLessonId);
                if (!result)
                {
                    string msg = $"cannot find student lesson with id: {studentLessonId} in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot remove the student lesson with the student lesson id: {studentLessonId} from DB. Due to {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
       
    }
}