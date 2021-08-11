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
        public Task<Lesson> GetLesson(string classCode);

        public Task<Lesson> GetLesson(int lessonId);
        public Task<List<Lesson>> GetLessonsByTeacherId(int teacherId);
        public Task<Lesson> AddLesson(Lesson lesson);
        public Task<Lesson> UpdateLesson(Lesson lesson);
        public Task<bool> DeleteLesson(int lessonId);
        public Task<bool> DeleteAllLessonsByTeacherId(int teacherId);

    }
}
