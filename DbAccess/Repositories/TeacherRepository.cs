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
        public async Task<Log> GetLogById(int logId)
        {
            try
            {
                return await _context.Logs.Where(x => x.Id == logId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get log from DB. log id: {logId}. due to: {e}");
                return null;
            }
        }

        public async Task<Log> AddLog(Log log)
        {
            try
            {
                _context.Add(log);
                await _context.SaveChangesAsync();
                return log;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add log to DB. due to: {e}");
                return null;
            }
        }

        public async Task<List<Log>> GetLosgByPersonId(int personId)
        {
            try
            {
                return await _context.Logs.Where(x => x.PersonId == personId).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get logs from DB. person id: {personId}. due to: {e}");
                return null;
            }
        }

        public async Task<bool> RemoveLogById(int logId)
        {
            try
            {
                var logToRemove = GetLogById(logId);
                _context.Remove(logToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot remove log from DB. log id: {logId}. due to: {e}");
                return false;
            }
        }
    }
}
