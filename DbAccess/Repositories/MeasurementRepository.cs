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
        private readonly IPersonRepository _personRepository;

        public MeasurementRepository(BackEyeContext context, ILogger<LogsRepository> logger, ILessonRepository lessonRepository, IStudentLessonRepository studentLessonRepository, IPersonRepository personRepository)
        {
            _context = context;
            _logger = logger;
            _lessonRepository = lessonRepository;
            _studentLessonRepository = studentLessonRepository;
            _personRepository = personRepository;
        }

        public async Task<Measurement> AddMeasurement(Measurement measurement)
        {
            try
            {
                //TODO: we might need to comment it in order to improve performance
                var measurementFromDb = await GetMeasurement(measurement.Id);
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
                    attendanceList.Add((student, firstMeasurement?.DateTime));
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get attandance from DB. due to: {e}");
            }

            return attendanceList;
        }

        /// <summary>
        /// Get all the measurements collected for a specific student and lesson
        /// </summary>
        /// <param name="lessonId">id of requested lesson</param>
        /// <param name="personId">id of the requested student</param>
        /// <param name="lessonTime">start time of the lesson</param>
        /// <returns>list of measurements related to the given lesson and student</returns>
        public async Task<List<Measurement>> GetStudentMeasurements(int lessonId, int personId, DateTime lessonTime)
        {
            List<Measurement> measurements = null;
            try
            {
                var studentlesson = await _studentLessonRepository.GetStudentLesson(lessonId, personId);
                if (studentlesson == null || studentlesson.Person == null || studentlesson.Lesson == null)
                {
                    throw new Exception($"Cannot find student lesson with student id: {personId} and lesson id: {lessonId}");
                }
                var lessonLengthSec = (studentlesson.Lesson.EndTime - studentlesson.Lesson.StartTime).TotalSeconds;
                var lessonEnd = lessonTime.AddSeconds(lessonLengthSec);

                measurements = await _context.Measurements.Where(m => m.LessonId == lessonId &&
                   m.PersonId == personId &&
                  (m.DateTime >= lessonTime && m.DateTime <= lessonEnd)).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get measurements from DB. due to: {e}");
            }

            return measurements;
        }

        /// <summary>
        /// Get all the measurements collected for all students in specfic lesson
        /// </summary>
        /// <param name="lessonId">id of requested lesson</param>
        /// <param name="lessonTime">start time of the lesson</param>
        /// <returns>list of measurements related to the given lesson</returns>
        public async Task<List<Measurement>> GetLessonMeasurements(int lessonId, DateTime lessonTime)
        {
            List<Measurement> measurements = null;
            try
            {
                //get all students subscribed to the lesson
                var students = await _studentLessonRepository.GetStudentsByLessonId(lessonId);
                if (students == null || students.Count == 0)
                {
                    throw new Exception($"Cannot find students in lesson with id: {lessonId}");
                }
                measurements = new List<Measurement>();

                //for each student get its all measurements of lesson
                foreach (var student in students)
                {
                    var studentMeasurements = await GetStudentMeasurements(lessonId, student.Id, lessonTime);
                    if (studentMeasurements != null)
                    {
                        measurements.AddRange(studentMeasurements);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get measurements from DB. due to: {e}");
            }

            return measurements;
        }

        public async Task<Measurement> GetMeasurement(int measurementId)
        {
            try
            {
                return await _context.Measurements.Where(x => x.Id == measurementId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get measurement from DB due to: {e}");
                return null;
            }
        }

        public async Task<bool> DeleteMeasurement(int measurementId)
        {
            try
            {
                var measurement = await GetMeasurement(measurementId);
                if (measurement == null)
                {
                    throw new Exception($"Measurement with id: {measurementId} not found in DB");
                }
                _context.Measurements.Remove(measurement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete measurement with id: {measurementId} from DB due to: {e}");
                return false;
            }
        }

        public async Task<bool> DeleteAllStudentMeasurement(int personId)
        {
            try
            {
                var allPersonMeasurement = await _context.Measurements.Where(m => m.PersonId == personId).ToListAsync();
                if (allPersonMeasurement == null)
                {
                    throw new Exception($"Measurement of person with id: {personId} not found in DB");
                }
                allPersonMeasurement.ForEach(m => _context.Measurements.Remove(m));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete measurement of person with id: {personId} from DB due to: {e}");
                return false;
            }
        }

        public async Task<bool> DeleteStudentLessonMeasurement(int lessonId, int personId)
        {
            try
            {
                var allPersonLessonMeasurement = await _context.Measurements.Where(m => m.LessonId == lessonId && m.PersonId == personId).ToListAsync();
                if (allPersonLessonMeasurement == null)
                {
                    throw new Exception($"Measurement of lesson id: {lessonId} and person with id: {personId} not found in DB");
                }
                allPersonLessonMeasurement.ForEach(m => _context.Measurements.Remove(m));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete measurement of lesson id: {lessonId} and person with id: {personId} from DB due to: {e}");
                return false;
            }
        }
    }
}
