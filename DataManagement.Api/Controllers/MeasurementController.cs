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
            //TODO: add validation for each filed
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
    }
}
