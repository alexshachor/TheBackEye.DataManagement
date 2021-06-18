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
    public class LessonRepository : ILessonRepository
    {
        private readonly BackEyeContext _context;
        private readonly ILogger _logger;
        public LessonRepository(BackEyeContext context, ILogger<LogsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Lesson> GetLesson(string classCode)
        {
            try
            {
                return await _context.Lessons.Where(x => x.ClassCode == classCode).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get lesson from DB. class code: {classCode}. due to: {e}");
                return null;
            }
        }

        public async Task<Lesson> AddLesson(Lesson lesson)
        {
            try
            {
                var lessonFromDb = await GetLesson(lesson.ClassCode);
                if (lessonFromDb == null)
                {
                    _context.Add(lesson);
                    await _context.SaveChangesAsync();
                    return lesson;
                }
                _logger.LogInformation($"Student with class code: {lessonFromDb.ClassCode} already exist");
                return lessonFromDb;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add lesson to DB. due to: {e}");
                return null;
            }
        }

        public async Task<Lesson> DeleteLessonByClassCode(string classCode)
        {
            var lesson = await GetLesson(classCode);
            if (lesson == null)
            {
                return null;
            }
            try
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete lesson from DB. class code: {classCode}. due to: {e}");
            }
            return lesson;
        }
    }
}
