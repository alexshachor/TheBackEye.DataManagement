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
        public Task<Teacher> AddTeacher(Teacher teacher);
        public Task<Teacher> RemoveTeacherByPassword(string password);
        public Task<Teacher> GetTeacherByPassword(string password);
    }
}
