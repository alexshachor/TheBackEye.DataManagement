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