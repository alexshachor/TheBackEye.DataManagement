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
    /// MeasurementController is responsible for all the Measurement's CRUD operations using API calls 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMeasurementRepository _measurementRepository;

        public MeasurementController(ILogger<MeasurementController> logger, IMeasurementRepository measurementRepository)
        {
            _logger = logger;
            _measurementRepository = measurementRepository;
        }

        /// <summary>
        /// Add a new Measurement to DB
        /// </summary>
        /// <param name="measurementDto">MeasurementDto object contains all of the Measurement's details which will be added to DB</param>
        /// <response code="200">MeasurementDto object contains all of the details from DB</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpPost]
        [ProducesResponseType(typeof(MeasurementDto), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MeasurementDto>> Post([FromBody] MeasurementDto measurementDto)
        {
            //validate request
            if (measurementDto == null)
            {
                string msg = $"measurementDto is null";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //add measurement to DB
                var measurement = await _measurementRepository.AddMeasurement(measurementDto.ToModel());
                if (measurement == null)
                {
                    string msg = $"cannot add measurement to DB";
                    _logger.LogError(msg);
                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
                }
                else
                {
                    return Ok(measurement.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot add measurement to DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Get list of students attendance in specfic lesson in specific time
        /// </summary>
        /// <param name="lessonId">id of the requested lesson</param>
        /// <param name="lessonTime">time of the requested lesson</param>
        /// <response code="200">List of StudentAttendanceDto, each contains person details and its time entrance to the lesson</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="404">NotFound - cannot find the lesson or any students in the lesson</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpGet("GetStudentsAttendance/{lessonId}/{lessonTime}")]
        [ProducesResponseType(typeof(List<StudentAttendanceDto>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<List<StudentAttendanceDto>>> GetStudentsAttendance(int lessonId, DateTime lessonTime)
        {
            //validate request
            if (lessonId < 0 || lessonTime == DateTime.MinValue)
            {
                string msg = $"lesson id: {lessonId} or lesson time: {lessonTime} are invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //add measurement to DB
                var attendanceList = await _measurementRepository.GetAttendance(lessonId,lessonTime);
                if (attendanceList == null)
                {
                    string msg = $"attendane list is null - cannot get attendance list from DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    var studentsAttendance = new List<StudentAttendanceDto>();
                    attendanceList.ForEach(x => studentsAttendance.Add(new StudentAttendanceDto {
                    Person = x.Item1.ToDto(),
                    EntranceTime = x.Item2
                    }));
                    return Ok(studentsAttendance);
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get attendance list from DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Get student's measurements in a given lesson and time
        /// </summary>
        /// <param name="lessonId">id of the requested lesson</param>
        /// <param name="personId">id of the requested student</param>
        /// <param name="lessonTime">time of the requested lesson</param>
        /// <response code="200">List of MeasurementDto, each contains measurements result in specfic time during the lesson</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="404">NotFound - cannot find the student or any measurements in the lesson</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpGet("GetStudentMeasurements/{lessonId}/{personId}/{lessonTime}")]
        [ProducesResponseType(typeof(List<MeasurementDto>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MeasurementDto>> GetStudentMeasurements(int lessonId, int personId, DateTime lessonTime)
        {
            //validate request
            if (lessonId < 0 || personId < 0 || lessonTime == DateTime.MinValue)
            {
                string msg = $"lesson id: {lessonId} or person id: {personId} or lesson time: {lessonTime} are invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get student measurement from DB
                var measurement = await _measurementRepository.GetStudentMeasurements(lessonId,personId,lessonTime);
                if (measurement == null)
                {
                    string msg = $"cannot get measurement from DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(measurement.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get measurement from DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Get lesson measurements of all students in a given lesson and time
        /// </summary>
        /// <param name="lessonId">id of the requested lesson</param>
        /// <param name="lessonTime">time of the requested lesson</param>
        /// <response code="200">List of MeasurementDto, each contains measurements result in specfic time during the lesson</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="404">NotFound - cannot find the student or any measurements in the lesson</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpGet("GetLessonMeasurements/{lessonId}/{lessonTime}")]
        [ProducesResponseType(typeof(List<MeasurementDto>), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(typeof(NotFoundResult), 404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<MeasurementDto>> GetLessonMeasurements(int lessonId, DateTime lessonTime)
        {
            //validate request
            if (lessonId < 0 || lessonTime == DateTime.MinValue)
            {
                string msg = $"lesson id: {lessonId} or lesson time: {lessonTime} are invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //get lesson measurements (of all students) from DB
                var measurement = await _measurementRepository.GetLessonMeasurements(lessonId, lessonTime);
                if (measurement == null)
                {
                    string msg = $"cannot get measurement from DB";
                    _logger.LogError(msg);
                    return NotFound(msg);
                }
                else
                {
                    return Ok(measurement.ToDto());
                }
            }
            catch (Exception e)
            {
                string msg = $"cannot get measurement from DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        /// <summary>
        /// Delete measurement from DB
        /// </summary>
        /// <param name="measurementId">id of the requested measurement</param>
        /// <response code="200">true - deletion was success</response>
        /// <response code="400">BadRequest - invalid values</response>
        /// <response code="404">NotFound - cannot find the measurement in DB</response>
        /// <response code="500">InternalServerError - for any error occurred in server</response>
        [HttpDelete("{measurementId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(BadRequestResult), 400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<bool>> Delete(int measurementId)
        {
            //validate request
            if (measurementId < 0 )
            {
                string msg = $"measurement id: {measurementId} is invalid";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            try
            {
                //delete measurement from DB
                var result = await _measurementRepository.DeleteMeasurement(measurementId);
                if (!result)
                {
                    string msg = $"cannot find measurement id: {measurementId} in DB";
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
                string msg = $"cannot delete measurement  from DB. due to: {e}";
                _logger.LogError(msg);
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }
    }
}
