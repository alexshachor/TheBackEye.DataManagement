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
    /// LogController is responsible for all the log's CRUD operations using API calls 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILogRepository _logRepository;

        public LogController(ILogger<LogController> logger, ILogRepository logRepository)
        {
            _logger = logger;
            _logRepository = logRepository;
        }

        /// <summary>
        /// Get LogDto object by the log id number
        /// </summary>
        /// <param name="logId">The identity number of the log</param>
        /// <response code="200">LogDto object contains all of the log's details</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the student in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(LogDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpGet("{birthId}")]
        public async Task<ActionResult<LogDto>> Get(int logId)
        {
            //validate request
            if (String.IsNullOrWhiteSpace(logId.ToString()))
            {
                string msg = $"logId: {logId} must not be null or empty";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get student from DB
                var log = await _logRepository.GetLogById(logId);
                if (log == null)
                {
                    string msg = $"log with log id: {logId} not found in DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(log.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get log with log id: {logId}. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }

        }

        /// <summary>
        /// Add a new log to DB
        /// </summary>
        /// <param name="logDto">LogDto object contains all of the log's details which will be added to DB</param>
        /// <response code="200">LogDto object contains all of the log's details from DB</response>
        /// <response code="400">BadRequest - invalid values (Student or Person is null)</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPost]
        [ProducesResponseType(typeof(LogDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<LogDto>> Post([FromBody] LogDto logDto)
        {
            if (logDto == null)
            {
                string msg = $"logDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //add log to DB
                var log = await _logRepository.AddLog(logDto.ToModel());
                if (log == null)
                {
                    string msg = $"cannot add the log with the next details - Person Id: {logDto.PersonId}" +
                        $" Creation Date: {logDto.CreationDate}  Data: {logDto.Data} to DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(log.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot add the log with the next details - person id: {logDto.PersonId}" +
                    $" Creation Date: {logDto.CreationDate}  Data: {logDto.Data} to DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        ///// <summary>
        ///// Add a list of new students to DB
        ///// </summary>
        ///// <param name="studentsDto">List of StudentDto object each one contains all of the student's personal details which will be added to DB</param>
        ///// <response code="200">List of StudentDto object each one contains all of the student's personal details from DB</response>
        ///// <response code="400">BadRequest - invalid values (students list is null or empty)</response>
        ///// <response code="500">InternalServerError - for any error occurred in server</response>
        //[HttpPost]
        //[ProducesResponseType(typeof(List<StudentDto>), 200)]
        //[ProducesResponseType(typeof(BadRequestResult), 400)]
        //[ProducesResponseType(500)]
        //public async Task<ActionResult<List<StudentDto>>> PostStudents([FromBody] List<StudentDto> studentsDto)
        //{
        //    //TODO: add validation for each filed
        //    //validate request
        //    if (studentsDto == null || studentsDto.Count == 0)
        //    {
        //        string msg = $"studentsDto list is null or empty";
        //        _logger.LogError(msg);
        //        return BadRequest(msg);
        //    }
        //    try
        //    {
        //        List<StudentDto> newStudents = new List<StudentDto>();
        //        //add each student to DB
        //        foreach (StudentDto studentDto in studentsDto)
        //        {
        //            var student = await _studentRepository.AddStudent(studentDto.ToModel());
        //            if (student == null)
        //            {
        //                string msg = $"cannot add student with birth id: {studentDto.BirthId} to DB";
        //                _logger.LogError(msg);
        //            }
        //            else
        //            {
        //                newStudents.Add(student.ToDto());
        //            }
        //        }
        //        return Ok(newStudents);
        //    }
        //    catch (Exception e)
        //    {
        //        string msg = $"cannot add students to DB. due to: {e}";
        //        _logger.LogError(msg);
        //        return StatusCode(StatusCodes.Status500InternalServerError, msg);
        //    }
        //}

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

