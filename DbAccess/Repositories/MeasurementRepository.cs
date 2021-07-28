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

        /// <summary>
        /// add new measurement to DB
        /// </summary>
        /// <param name="measurement">measurement to add</param>
        /// <returns>the newly added measurement</returns>
        public async Task<Measurement> AddMeasurement(Measurement measurement)
        {
            try
            {
                //in order to improve performance we just add it to DB
                _context.Add(measurement);
                await _context.SaveChangesAsync();
                return measurement;
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

        /// <summary>
        /// get measurement from DB by the given measurement id
        /// </summary>
        /// <param name="measurementId">measurement id</param>
        /// <returns>the requested measurement from DB</returns>
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

        /// <summary>
        /// delete measurement from DB by the given measurement id 
        /// </summary>
        /// <param name="measurementId">measurement id</param>
        /// <returns>true if deletion was a success and false otherwise</returns>
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

        /// <summary>
        /// delete from DB all the measurement of a given person id 
        /// </summary>
        /// <param name="personId">person id</param>
        /// <returns>true if deletion was a success and false otherwise</returns>
        public async Task<bool> DeleteAllStudentMeasurement(int personId)
        {
            try
            {
                var allPersonMeasurement = await _context.Measurements.Where(m => m.PersonId == personId).ToListAsync();
                if (allPersonMeasurement == null || allPersonMeasurement.Count == 0)
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

        /// <summary>
        /// delete from DB all the measurement of a person of a specfic lesson
        /// </summary>
        /// <param name="lessonId">person id</param>
        /// <param name="personId">lesson id</param>
        /// <returns>true if deletion was a success and false otherwise</returns>
        public async Task<bool> DeleteStudentLessonMeasurement(int lessonId, int personId)
        {
            try
            {
                var allPersonLessonMeasurement = await _context.Measurements.Where(m => m.LessonId == lessonId && m.PersonId == personId).ToListAsync();
                if (allPersonLessonMeasurement == null || allPersonLessonMeasurement.Count == 0)
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

        /// <summary>
        /// get the dates of all the lessons which took place 
        /// </summary>
        /// <param name="lessonId">lesson id</param>
        /// <returns>loist of datetime, each represent a lesson start time</returns>
        public async Task<List<DateTime>> GetLessonDates(int lessonId)
        {
            List<DateTime> dates = null;
            const int weeksNumber = 53;
            try
            {
                var lesson = await _lessonRepository.GetLesson(lessonId);
                if (lesson == null)
                {
                    throw new Exception($"Cannot find lesson with id: {lessonId}");
                }
                dates = new List<DateTime>();
                DateTime current = lesson.StartTime;
                for (int i = 0; i < weeksNumber; i++)
                {
                    var measurements = await GetLessonMeasurements(lessonId, current);
                    //if there is any measurment taken in that time => there has been a lesson
                    if (measurements != null && measurements.Count > 0)
                    {
                        dates.Add(current);
                    }
                    current = current.AddDays(7);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get lesson from DB. lesson id: {lessonId}. due to: {e}");
            }
            return dates;
        }
    }
}
