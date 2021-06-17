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
    /// StudentController is responsible for all the student's CRUD operations using API calls 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILessonRepository _lessonRepository;

        public LessonController(ILogger<LessonController> logger, ILessonRepository lessonRepository)
        {
            _logger = logger;
            _lessonRepository = lessonRepository;
        }

        /// <summary>
        /// Get LessonDto object by class code string
        /// </summary>
        /// <param name="classCode">The class code string</param>
        /// <response code="200">LessonDto object contains all of the lesson details</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the student in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(LessonDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpGet("{classCode}")]
        public async Task<ActionResult<LessonDto>> Get(string classCode)
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
                var lesson = await _lessonRepository.GetLesson(classCode);
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
    }
}
