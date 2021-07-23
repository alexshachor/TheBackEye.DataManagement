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
        public MeasurementRepository(BackEyeContext context, ILogger<LogsRepository> logger)
        {
            _context = context;
            _logger = logger;
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
    }
}
