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
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly BackEyeContext _context;
        private readonly ILogger _logger;
        private readonly ILessonRepository _lessonRepository;
        private readonly IStudentLessonRepository _studentLessonRepository;

        public MeasurementRepository(BackEyeContext context, ILogger<LogsRepository> logger, ILessonRepository lessonRepository, IStudentLessonRepository studentLessonRepository)
        {
            _context = context;
            _logger = logger;
            _lessonRepository = lessonRepository;
            _studentLessonRepository = studentLessonRepository;
        }

        public async Task<Measurement> GetMeasurement(int lessonId, int personId, DateTime dateTime)
        {
            try
            {
                return await _context.Measurements.Where(x => x.LessonId == lessonId && x.PersonId == personId && x.DateTime == dateTime).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get measurement from DB due to: {e}");
                return null;
            }
        }

        public async Task<Measurement> AddMeasurement(Measurement measurement)
        {
            try
            {
                //TODO: we might need to comment it in order to improve performance
                var measurementFromDb = await GetMeasurement(measurement.LessonId, measurement.PersonId, measurement.DateTime);
                if (measurementFromDb == null)
                {
                    _context.Add(measurement);
                    await _context.SaveChangesAsync();
                    return measurement;
                }
                _logger.LogInformation($"Measurement already exist");
                return measurementFromDb;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add measurement to DB. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// Get attandance of each student including time of enterance
        /// </summary>
        /// <param name="lessonId">id of requested lesson</param>
        /// <param name="lessonTime">exact time of the beginning of the lesson</param>
        /// <returns>list of tuples, each contains the student and its datetime of entrance (null if not exist)</returns>
        public async Task<List<(Person, DateTime?)>> GetAttendance(int lessonId, DateTime lessonTime)
        {
            List<(Person, DateTime?)> attendanceList = null;
            try
            {
                var lesson = await _lessonRepository.GetLesson(lessonId);
                if (lesson == null)
                {
                    throw new Exception($"Cannot find lesson with id: {lessonId}");
                }
                //get all students subscribed to the lesson
                var students = await _studentLessonRepository.GetStudentsByLessonId(lessonId);
                if (students == null || students.Count == 0)
                {
                    throw new Exception($"Cannot find students of lesson id: {lessonId}");
                }
                attendanceList = new List<(Person, DateTime?)>();
                var maxLateDateTime = lessonTime.AddMinutes(lesson.MaxLate);

                //for each student get its first measurement of lesson in valid range of date (not too late)
                foreach (var student in students)
                {
                    var firstMeasurement = await _context.Measurements.Where(m => m.LessonId == lessonId &&
                    m.PersonId == student.Id &&
                   (m.DateTime >= lessonTime && m.DateTime <= maxLateDateTime)).FirstOrDefaultAsync();
                    attendanceList.Add((student,firstMeasurement?.DateTime));
                }           
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get attandance from DB. due to: {e}");
            }

            return attendanceList;
        }
    }
}
