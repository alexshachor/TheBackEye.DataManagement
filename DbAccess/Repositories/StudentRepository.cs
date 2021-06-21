//using DbAccess.RepositoryInterfaces;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DbAccess.Repositories
//{
//    /// <summary>
//    /// this class manages all student's CRUD operations against the DB
//    /// </summary>
//    public class StudentRepository : IStudentRepository
//    {
//        private readonly BackEyeContext _context;
//        private readonly ILogger _logger;
//        public StudentRepository(BackEyeContext context, ILogger<StudentRepository> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        /// <summary>
//        /// add new student to DB
//        /// </summary>
//        /// <param name="student">student to add</param>
//        /// <returns>Student object if success and null otherwise</returns>
//        public async Task<Student> AddStudent(Student student)
//        {
//            if (student == null)
//            {
//                _logger.LogError($"Cannot add student to DB - student is null");
//                return null;
//            }
//            try
//            {
//                var studentFromDb = await GetStudentByBirthId(student.BirthId);
//                if (studentFromDb == null)
//                {
//                    _context.Add(student);
//                    await _context.SaveChangesAsync();
//                    return student;
//                }
//                _logger.LogInformation($"Student with birth id: {studentFromDb.BirthId} already exist");
//                return studentFromDb;
//            }
//            catch (Exception e)
//            {
//                _logger.LogError($"Cannot add student to DB. due to: {e}");
//                return null;
//            }
//        }

//        /// <summary>
//        /// get a student from DB by the given id
//        /// </summary>
//        /// <param name="studentId">the id of the student</param>
//        /// <returns>Student object if success and null otherwise</returns>
//        public async Task<Student> GetStudentById(int studentId)
//        {
//            try
//            {
//                return await _context.Students.Where(x => x.Id == studentId).Include(x => x.Person).FirstOrDefaultAsync();
//            }
//            catch (Exception e)
//            {
//                _logger.LogError($"Cannot get student from DB. student id: {studentId}. due to: {e}");
//                return null;
//            }
//        }

//        /// <summary>
//        /// get a list of students by the given class id
//        /// </summary>
//        /// <param name="classId">the id of the class</param>
//        /// <returns>List of Student object if success and null otherwise</returns>
//        public async Task<List<Student>> GetStudentsByClassId(int classId)
//        {
//            try
//            {
//                var studentIds = await _context.StudentClasses.Where(x => x.ClassId == classId).Select(x => x.StudentId).ToListAsync();
//                List<Student> students = new List<Student>();
//                foreach (var id in studentIds)
//                {
//                    var student = await GetStudentById(id);
//                    students.Add(student);
//                }
//                return students;
//            }
//            catch (Exception e)
//            {
//                _logger.LogError($"Cannot get students from DB by class id. class id: {classId}. due to: {e}");
//                return null;
//            }
//        }

//        /// <summary>
//        /// check if a given student is exist in DB
//        /// </summary>
//        /// <param name="studentId">the student id</param>
//        /// <returns>true if exist and false otherwise</returns>
//        public async Task<bool> IsStudentExist(int studentId)
//        {
//            return await _context.Students.AnyAsync(s => s.Id == studentId);
//        }

//        /// <summary>
//        /// delete student by the given id
//        /// </summary>
//        /// <param name="studentId">the id of the student to delete</param>
//        /// <returns>true if success and false otherwise</returns>
//        public async Task<bool> DeleteStudentById(int studentId)
//        {
//            var student = await GetStudentById(studentId);
//            if (student == null)
//            {
//                return false;
//            }
//            try
//            {
//                _context.Students.Remove(student);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch (Exception e)
//            {
//                _logger.LogError($"Cannot delete student from DB. student id: {studentId}. due to: {e}");
//            }
//            return false;
//        }

//        /// <summary>
//        /// update the student entity eith the given student
//        /// </summary>
//        /// <param name="student">Student object with updated data</param>
//        /// <returns>Student object if success and null otherwise</returns>
//        public async Task<Student> UpdateStudent(Student student)
//        {
//            if (student == null)
//            {
//                _logger.LogError($"Cannot update student in DB - student is null");
//                return null;
//            }
//            _context.Entry(student.Person).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//                return student;
//            }
//            catch (Exception e)
//            {
//                _logger.LogError($"Cannot update student in DB. student id: {student.Id}. due to: {e}");
//                return null;
//            }
//        }

//        /// <summary>
//        /// get a student from DB by the given birth id
//        /// </summary>
//        /// <param name="studentBirthId">the birth id of the student</param>
//        /// <returns>Student object if success and null otherwise</returns>
//        public async Task<Student> GetStudentByBirthId(string studentBirthId)
//        {
//            try
//            {
//                return await _context.Students.Where(x => x.BirthId == studentBirthId).Include(x => x.Person).FirstOrDefaultAsync();
//            }
//            catch (Exception e)
//            {
//                _logger.LogError($"Cannot get student from DB. student id: {studentBirthId}. due to: {e}");
//                return null;
//            }
//        }

//        /// <summary>
//        /// check if a given student is exist in DB
//        /// </summary>
//        /// <param name="birthId">the birth id of the student</param>
//        /// <returns>true if exist and false otherwise</returns>
//        public async Task<bool> IsStudentExistByBirthId(string birthId)
//        {
//            return await _context.Students.AnyAsync(s => s.BirthId == birthId);
//        }
//    }
//}
