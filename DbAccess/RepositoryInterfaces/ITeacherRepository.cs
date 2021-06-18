using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface ITeacherRepository
    {
        public Task<Log> AddLog(Log log);
        public Task<bool> RemoveLogById(int logId);
        public Task<Teacher> GetTeacherByPassword(string password);
        public Task<List<Log>> GetLosgByPersonId(int personId);

    }
}
