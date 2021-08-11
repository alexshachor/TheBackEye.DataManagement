using BusinessLogic;
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

namespace DataManagement.Api.Controllers
{
    /// <summary>
    /// LessonController is responsible for all the leasson's CRUD operations using API calls 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILessonRepository _lessonRepository;
        private readonly IMeasurementRepository _measurementRepository;


        public LessonController(ILogger<LessonController> logger, ILessonRepository lessonRepository, IMeasurementRepository measurementRepository)
        {
            _logger = logger;
            _lessonRepository = lessonRepository;
            _measurementRepository = measurementRepository;
        }

        /// <summary>
        /// Get LessonDto object by lesson id
        /// </summary>
        /// <param name="lessonId">Lesson id</param>
        /// <response code="200">LessonDto object contains all of the lesson details</response>
        /// <response code="400">BadRequest - invalid values (null or empty class code)</response>
        /// <response code="404">NotFound - cannot find the lesson in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(LessonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpGet("{lessonId}")]
        public async Task<ActionResult<LessonDto>> Get(int lessonId)
        {
            //validate request
            if (lessonId < 1)
            {
                string msg = $"lesson id: {lessonId} must be positive";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get lesson from DB
                var lesson = await _lessonRepository.GetLesson(lessonId);
                if (lesson == null)
                {
                    string msg = $"lesson with lesson id: {lessonId} not found in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(lesson.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get lesson with lesson id: {lessonId}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Get LessonDto object (contains the next lesson's dates) by class code string
        /// </summary>
        /// <param name="classCode">The class code string</param>
        /// <response code="200">LessonDto object contains all of the lesson details</response>
        /// <response code="400">BadRequest - invalid values (null or empty class code)</response>
        /// <response code="404">NotFound - cannot find the lesson in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(LessonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpGet("GetNextLesson/{classCode}")]
        public async Task<ActionResult<LessonDto>> GetNextLesson(string classCode)
        {
            //validate request
            if (String.IsNullOrWhiteSpace(classCode))
            {
                string msg = $"class code: {classCode} must not be null or empty";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get lesson from DB
                var lesson = await _measurementRepository.GetNextLesson(classCode);
                if (lesson == null)
                {
                    string msg = $"lesson with class code: {classCode} not found in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(lesson.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get lesson with the class code: {classCode}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Get list of LessonDto object related to teacher by the given teacher id
        /// </summary>
        /// <param name="teacherId">The id of the teacher</param>
        /// <response code="200">List of LessonDto object, each one contains all of the lesson details</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the related lessons in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(List<LessonDto>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpGet("AllLessons/{teacherId}")]
        public async Task<ActionResult<List<LessonDto>>> GetLessons(int teacherId)
        {
            //validate request
            if (teacherId < 1)
            {
                string msg = $"teacher id: {teacherId} must be positive";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get lessons from DB
                var lessons = await _lessonRepository.GetLessonsByTeacherId(teacherId);
                if (lessons == null)
                {
                    string msg = $"lessons with teacher id: {teacherId} not found in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(lessons.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get lessons related to teacher id: {teacherId}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Add a new lesson to DB
        /// </summary>
        /// <param name="lessonDto">LessonDto object contains all of the lesson's details which will be added to DB</param>
        /// <response code="200">LessonDto object contains all of the details from DB</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPost]
        [ProducesResponseType(typeof(LessonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LessonDto>> Post([FromBody] LessonDto lessonDto)
        {
            //validate request
            if (lessonDto == null)
            {
                string msg = $"lessonDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            if (lessonDto.Id < 0 || lessonDto.PersonId < 1)
            {
                string msg = $"lessonDto.id: {lessonDto.Id} or " +
                    $"lessonDto.PersonId: {lessonDto.PersonId} or are invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                if (string.IsNullOrEmpty(lessonDto.ClassCode))
                {
                    lessonDto.ClassCode = PasswordGenerator.Generate();
                }
                //add a new lesson to DB
                var lesson = await _lessonRepository.AddLesson(lessonDto.ToModel());
                if (lesson == null)
                {
                    string msg = $"cannot add lesson with lesson id: {lessonDto.Id} to DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(lesson.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot add lesson with lesson id: {lessonDto.Id} to DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

       
        /// <summary>
        /// Change lesson in the DB
        /// </summary>
        /// <param name="lessonDto">LessonDto object contains all of the new lesson's details</param>
        /// <response code="200">LessonDto object contains all of the details from DB</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPut]
        [ProducesResponseType(typeof(LessonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LessonDto>> Put([FromBody] LessonDto lessonDto)
        {
            //validate request
            if (lessonDto == null)
            {
                string msg = $"lessonDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            if (lessonDto.Id < 0 || lessonDto.PersonId < 1 || string.IsNullOrEmpty(lessonDto.ClassCode))
            {
                string msg = $"lessonDto.id: {lessonDto.Id} or " +
                    $"lessonDto.PersonId: {lessonDto.PersonId} or" +
                    $" lessonDto.ClassCode {lessonDto.ClassCode} are invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //update lesson in DB
                var lesson = await _lessonRepository.UpdateLesson(lessonDto.ToModel());
                if (lesson == null)
                {
                    string msg = $"cannot change the lesson with lesson id: {lessonDto.Id}";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(lesson.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot change the lesson with lesson id: {lessonDto.Id}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Delete lessonDto object by lesson id
        /// </summary>
        /// <param name="lessonId">The lesson we want to delete </param>
        /// <response code="200">bool true</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the lesson in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpDelete("{lessonId}")]
        public async Task<ActionResult<bool>> Delete(int lessonId)
        {
            if (lessonId < 0)
            {
                string msg = $"lesson id: {lessonId} must be positive";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //remove lesson from DB
                var tmpLesson = await _lessonRepository.DeleteLesson(lessonId);
                if (tmpLesson == false)
                {
                    string msg = $"cannot remove the lesson id: {lessonId} from DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot remove the lesson with lesson id: {lessonId} from DB. Due to {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

    }
}
