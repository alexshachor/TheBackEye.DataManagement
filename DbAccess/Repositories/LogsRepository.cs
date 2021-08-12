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
    /// <summary>
    /// this class manages all log's CRUD operations against the DB
    /// </summary>
    public class LogsRepository : ILogRepository
    {
        private readonly BackEyeContext _context;
        private readonly ILogger _logger;
        public LogsRepository(BackEyeContext context, ILogger<LogsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// get log by its id
        /// </summary>
        /// <param name="logId">the log id</param>
        /// <returns>Log object if success and null otherwise</returns>
        public async Task<Log> GetLogById(int logId)
        {
            try
            {
                return await _context.Logs.Where(x => x.Id == logId).Include(l => l.Person).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get log from DB. log id: {logId}. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// add a given new log to DB
        /// </summary>
        /// <param name="log">log to add</param>
        /// <returns>Log object if success and null otherwise</returns>
        public async Task<Log> AddLog(Log log)
        {
            try
            {
                _context.Add(log);
                await _context.SaveChangesAsync();
                return await GetLogById(log.Id);
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add log to DB. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// get all the logs of a given person id
        /// </summary>
        /// <param name="personId">id of person</param>
        /// <returns>List of Log object if success and null otherwise</returns>
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

        /// <summary>
        /// remove log by the given id
        /// </summary>
        /// <param name="logId">id pf log to remove</param>
        /// <returns>true if deletion was a success and false otherwise</returns>
        public async Task<bool> RemoveLogById(int logId)
        {
            try
            {
                var logToRemove = await GetLogById(logId);
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
        /// <summary>
        /// remove log by the given id
        /// </summary>
        /// <param name="logId">id pf log to remove</param>
        /// <returns>true if deletion was a success and false otherwise</returns>
        public async Task<bool> DeletePersonLogs(int personId)
        {
            try
            {
                var logsToRemove = await _context.Logs.Where(l => l.PersonId == personId).ToListAsync();
                _context.Remove(logsToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot remove logs of a person from DB. person id: {personId}. due to: {e}");
                return false;
            }
        }
    }
}
