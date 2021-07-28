using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface ILogRepository
    {
        public Task<Log> AddLog(Log log);
        public Task<bool> RemoveLogById(int logId);
        public Task<Log> GetLogById(int logId);
        public Task<List<Log>> GetLosgByPersonId(int personId);

        public Task<bool> DeletePersonLogs(int personId);

    }
}
