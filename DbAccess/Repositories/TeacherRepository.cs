using DbAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly BackEyeContext _context;
        private readonly ILogger _logger;
        public TeacherRepository(BackEyeContext context, ILogger<TeacherRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Teacher> GetTeacherByPassword(string password)
        {
            try
            {
                return await _context.Teachers.Where(x => x.Password == password).Include(x => x.Person).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get teacher from DB. teacher password: {password}. due to: {e}");
                return null;
            }
        }

        public async Task<Teacher> AddTeacher(Teacher teacher)
        {
            try
            {
                var teacherFromDb = await GetTeacherByPassword(teacher.Password);
                if (teacherFromDb == null)
                {
                    _context.Add(teacher);
                    await _context.SaveChangesAsync();
                    return teacher;
                }
                _logger.LogInformation($"Teacher with password: {teacherFromDb.Password} already exist");
                return teacherFromDb;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add teacher to DB. due to: {e}");
                return null;
            }
        }

        public async Task<Teacher> RemoveTeacherByPassword(string password)
        {
            var teacher = await GetTeacherByPassword(password);
            if (teacher == null)
            {
                return null;
            }
            try
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete teacher from DB. password: {password}. due to: {e}");
            }
            return teacher;
        }
    }
}
