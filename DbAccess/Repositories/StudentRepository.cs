using DbAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly BackEyeContext _context;
        private readonly ILogger _logger;
        public StudentRepository(BackEyeContext context, ILogger<StudentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<Student> AddStudent(Student student)
        {
            try
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return student;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add student to DB. due to: {e}");
                return null;
            }
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            try
            {
                return await _context.Students.Where(x => x.Id == studentId).Include(x => x.Person).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get student from DB. student id: {studentId}. due to: {e}");
                return null;
            }
        }

        public async Task<List<Student>> GetStudentsByClassId(int classId)
        {
            try
            {
                var studentIds = await _context.StudentClasses.Where(x => x.ClassId == classId).Select(x => x.StudentId).ToListAsync();
                List<Student> students = new List<Student>();
                foreach (var id in studentIds)
                {
                    var student = await GetStudentById(id);
                    students.Add(student);
                }
                return students;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get students from DB by class id. class id: {classId}. due to: {e}");
                return null;
            }
        }

        public async Task<bool> IsStudentExist(int studentId)
        {
            return await _context.Students.AnyAsync(s => s.Id == studentId);
        }

        public async Task<Student> DeleteStudentById(int studentId)
        {
            var student = await GetStudentById(studentId);
            if (student == null)
            {
                return null;
            }
            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete student from DB. student id: {studentId}. due to: {e}");
            }
            return student;
        }

        public async Task<Student> UpdateStudent(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return student;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot update student in DB. student id: {student.Id}. due to: {e}");
                return null;
            }
        }

        public async Task<Student> GetStudentByBirthId(int studentBirthId)
        {
            try
            {
                return await _context.Students.Where(x => x.IdNumber == studentBirthId).Include(x => x.Person).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get student from DB. student id: {studentBirthId}. due to: {e}");
                return null;
            }
        }
    }
}
