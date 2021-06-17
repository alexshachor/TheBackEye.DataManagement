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
        [HttpGet("{logId}")]
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

        /// <summary>
        /// Delete LogDto object by the log id number
        /// </summary>
        /// <param name="logId">The identity number of the log</param>
        /// <response code="200">bool true</response>
        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
        /// <response code="404">NotFound - cannot find the student in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        [HttpDelete("{logId}")]
        public async Task<ActionResult<bool>> Delete(int logId)
        {
            if (String.IsNullOrWhiteSpace(logId.ToString()))
            {
                string msg = $"logDto is null or empty";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //remove log from DB
                var res = await _logRepository.RemoveLogById(logId);
                if (res == false)
                {
                    string msg = $"cannot remove the log with the Log Id: {logId} from DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot remove the log with the Log Id: {logId} from DB";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
    }
}

