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
    public class StudentLessonRepository : IStudentLessonRepository
    {
        private readonly BackEyeContext _context;
        private readonly ILogger _logger;
        public StudentLessonRepository(BackEyeContext context, ILogger<LogsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<StudentLesson> GetStudentLesson(int lessonId, int personId)
        {
            try
            {
                return await _context.StudentLessons.Where(x => x.LessonId == lessonId && x.PersonId == personId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get student lesson from DB. lesson id: {lessonId}. due to: {e}");
                return null;
            }
        }

        public async Task<StudentLesson> AddStudentLesson(StudentLesson studentLesson)
        {
            try
            {
                var studentLessonFromDb = await GetStudentLesson(studentLesson.LessonId, studentLesson.PersonId);
                if (studentLessonFromDb == null)
                {
                    _context.Add(studentLesson);
                    await _context.SaveChangesAsync();
                    return studentLesson;
                }
                _logger.LogInformation($"Student Lesson with Lesson Id: {studentLessonFromDb.LessonId} already exist");
                return studentLessonFromDb;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add student lesson to DB. due to: {e}");
                return null;
            }
        }

        public async Task<Lesson> DeleteLesson(Lesson lesson)
        {
            var tmpLesson = await GetLesson(lesson.ClassCode);
            if (tmpLesson == null)
            {
                return null;
            }
            try
            {
                _context.Lessons.Remove(tmpLesson);
                _context.StudentClasses.RemoveRange(_context.StudentClasses.Where(x => x.LessonId == tmpLesson.Id && x.PersonId == tmpLesson.PersonId));
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete lesson from DB. class code: {tmpLesson.ClassCode}. due to: {e}");
            }
            return lesson;
        }
    }
}
