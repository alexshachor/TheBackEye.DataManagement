using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface IMeasurementRepository
    {

        public Task<Measurement> GetMeasurement(int measurementId);

        public Task<List<Measurement>> GetStudentMeasurements(int lessonId, int personId, DateTime lessonTime);

        public Task<List<Measurement>> GetLessonMeasurements(int lessonId, DateTime lessonTime);

        public Task<List<DateTime>> GetLessonDates(int lessonId);

        public Task<Measurement> AddMeasurement(Measurement measurement);

        public Task<List<(Person, DateTime?)>> GetAttendance(int lessonId, DateTime lessonTime);

        public Task<bool> DeleteMeasurement(int measurementId);

        public Task<bool> DeleteAllStudentMeasurement(int personId);

        public Task<bool> DeleteStudentLessonMeasurement(int lessonId, int personId);
    }
}
