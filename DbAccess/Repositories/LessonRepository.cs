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

        /// <summary>
        /// get a lesson from DB by class code identifier
        /// </summary>
        /// <param name="classCode">class code</param>
        /// <returns>the requested lesson</returns>
        public async Task<Lesson> GetLesson(string classCode)
        {
            try
            {
                return await _context.Lessons.Where(x => x.ClassCode == classCode).Include(x=>x.Person).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get lesson from DB. class code: {classCode}. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// get all the lessons related to a given teacher
        /// </summary>
        /// <param name="teacherId">person id represents the teacher</param>
        /// <returns>list of lesson object all related to the teacher</returns>
        public async Task<List<Lesson>> GetLessonsByTeacherId(int teacherId)
        {
            try
            {
                return await _context.Lessons.Where(x => x.PersonId == teacherId).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get lessons from DB. teacher id: {teacherId}. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// add a new lesson to DB
        /// </summary>
        /// <param name="lesson">lesson to add</param>
        /// <returns>the newly added lesson</returns>
        public async Task<Lesson> AddLesson(Lesson lesson)
        {
            try
            {
                var lessonFromDb = await GetLesson(lesson.ClassCode);
                if (lessonFromDb == null)
                {
                    lesson.Person = null;
                    _context.Add(lesson);
                    await _context.SaveChangesAsync();
                    lessonFromDb = await GetLesson(lesson.ClassCode);                 
                }
                else
                {
                    _logger.LogInformation($"Class code: {lessonFromDb.ClassCode} already exist");
                }
                return lessonFromDb;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add lesson to DB. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// delete a given lesson from DB
        /// </summary>
        /// <param name="lesson">lesson object to delete</param>
        /// <returns>null if not found, lesson deleted otherwise</returns>
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
                _context.StudentLessons.RemoveRange(_context.StudentLessons.Where(x => x.LessonId == tmpLesson.Id && x.PersonId == tmpLesson.PersonId));
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete lesson from DB. class code: {tmpLesson.ClassCode}. due to: {e}");
            }
            return lesson;
        }

        /// <summary>
        /// Update lesson in DB
        /// </summary>
        /// <param name="lesson">lesson object contains details to update</param>
        /// <returns>the newly updated lesson</returns>
        public async Task<Lesson> UpdateLesson(Lesson lesson)
        {
            _context.Entry(lesson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return lesson;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot update lesson in DB. lesson id: {lesson.Id}. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// Get a lesson from Db by the given lesson id 
        /// </summary>
        /// <param name="lessonId">lesson id</param>
        /// <returns>lesson object contains all the details of a lesson</returns>
        public async Task<Lesson> GetLesson(int lessonId)
        {
            try
            {
                return await _context.Lessons.Where(x => x.Id == lessonId).Include(x => x.Person).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get lesson from DB. lesson id: {lessonId}. due to: {e}");
                return null;
            }
        }
    }
}
