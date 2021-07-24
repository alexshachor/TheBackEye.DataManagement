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
        public Task<Measurement> GetMeasurement(int lessonId, int personId, DateTime dateTime);

        public Task<Measurement> AddMeasurement(Measurement measurement);

        public Task<List<(Person, DateTime?)>> GetAttendance(int lessonId, DateTime lessonTime);
    }
}
