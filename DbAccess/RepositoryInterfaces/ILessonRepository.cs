using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface ILessonRepository
    {
        public Task<Lesson> GetLesson(int classId);
    }
}
