//using DbAccess.RepositoryInterfaces;
//using Dtos;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;


//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace StudentConfiguration.Api.Controllers
//{
//    /// <summary>
//    /// StudentController is responsible for all the student's CRUD operations using API calls 
//    /// </summary>
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StudentController : ControllerBase
//    {
//        private readonly ILogger _logger;
//        private readonly IStudentRepository _studentRepository;


//        public StudentController(ILogger<StudentController> logger, IStudentRepository studentRepository)
//        {
//            _logger = logger;
//            _studentRepository = studentRepository;
//        }

//        /// <summary>
//        /// Get StudentDto object by the student id number
//        /// </summary>
//        /// <param name="birthId">The identity number (from birth) of the student</param>
//        /// <response code="200">StudentDto object contains all of the student's personal details</response>
//        /// <response code="400">BadRequest - invalid values (lower than 1)</response>
//        /// <response code="404">NotFound - cannot find the student in DB</response>
//        /// <response code="500">InternalServerError - for any error occurred in server</response>
//        [ProducesResponseType(typeof(StudentDto), 200)]
//        [ProducesResponseType(typeof(BadRequestResult), 400)]
//        [ProducesResponseType(typeof(NotFoundResult), 404)]
//        [ProducesResponseType(500)]
//        [HttpGet("{birthId}")]
//        public async Task<ActionResult<StudentDto>> Get(string birthId)
//        {
//            //validate request
//            if (String.IsNullOrWhiteSpace(birthId))
//            {
//                string msg = $"birthId: {birthId} must not be null or empty";
//                _logger.LogError(msg);
//                return BadRequest(msg);
//            }
//            try
//            {
//                //get student from DB
//                var student = await _studentRepository.GetStudentByBirthId(birthId);
//                if (student == null)
//                {
//                    string msg = $"student with birth id: {birthId} not found in DB";
//                    _logger.LogError(msg);
//                    return NotFound(msg);
//                }
//                else
//                {
//                    return Ok(student.ToDto());
//                }
//            }
//            catch (Exception e)
//            {
//                string msg = $"cannot get student with birth id: {birthId}. due to: {e}";
//                _logger.LogError(msg);
//                return StatusCode(StatusCodes.Status500InternalServerError, msg);
//            }

//        }

//        /// <summary>
//        /// Add a new student to DB
//        /// </summary>
//        /// <param name="studentDto">StudentDto object contains all of the student's personal details which will be added to DB</param>
//        /// <response code="200">StudentDto object contains all of the student's personal details from DB</response>
//        /// <response code="400">BadRequest - invalid values (Student or Person is null)</response>
//        /// <response code="500">InternalServerError - for any error occurred in server</response>
//        [HttpPost]
//        [ProducesResponseType(typeof(StudentDto), 200)]
//        [ProducesResponseType(typeof(BadRequestResult), 400)]
//        [ProducesResponseType(500)]
//        public async Task<ActionResult<StudentDto>> Post([FromBody] StudentDto studentDto)
//        {
//            //TODO: add validation for each filed
//            //validate request
//            if (studentDto == null || studentDto.Person == null)
//            {
//                string msg = $"studentDto or personDto is null";
//                _logger.LogError(msg);
//                return BadRequest(msg);
//            }
//            try
//            {
//                //add student to DB
//                var student = await _studentRepository.AddStudent(studentDto.ToModel());
//                if (student == null)
//                {
//                    string msg = $"cannot add student with birth id: {studentDto.BirthId} to DB";
//                    _logger.LogError(msg);
//                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
//                }
//                else
//                {
//                    return Ok(student.ToDto());
//                }
//            }
//            catch (Exception e)
//            {
//                string msg = $"cannot add student with birth id: {studentDto.BirthId} to DB. due to: {e}";
//                _logger.LogError(msg);
//                return StatusCode(StatusCodes.Status500InternalServerError, msg);
//            }
//        }


    

//        /// <summary>
//        /// Delete student from DB by its id
//        /// </summary>
//        /// <param name="id">id of the student to delete</param>
//        /// <response code="200">Ok - student deletion was success</response>
//        /// <response code="400">BadRequest - invalid values (id is not in valid range)</response>
//        /// <response code="404">NotFound - cannot find the student in DB</response>
//        /// <response code="500">InternalServerError - for any error occurred in server</response>
//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            if (id < 0)
//            {
//                string msg = $"id: {id} is not valid";
//                _logger.LogError(msg);
//                return BadRequest(msg);
//            }
//            try
//            {
//                //delete student from DB
//                var isExist = await _studentRepository.IsStudentExist(id);
//                if (!isExist)
//                {
//                    string msg = $"cannot delete student with id: {id} from DB. student was not found.";
//                    _logger.LogError(msg);
//                    return NotFound(msg);
//                }
//                bool isDeleted = await _studentRepository.DeleteStudentById(id);
//                if (!isDeleted)
//                {
//                    string msg = $"cannot delete student with id: {id} from DB";
//                    _logger.LogError(msg);
//                    return StatusCode(StatusCodes.Status500InternalServerError, msg);
//                }
//                else
//                {
//                    return Ok();
//                }
//            }
//            catch (Exception e)
//            {
//                string msg = $"cannot delete student with id: {id} from DB. due to: {e}";
//                _logger.LogError(msg);
//                return StatusCode(StatusCodes.Status500InternalServerError, msg);
//            }
//        }
//    }
//}
