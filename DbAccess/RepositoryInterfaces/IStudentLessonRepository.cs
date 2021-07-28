using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface IStudentLessonRepository
    {
        public Task<List<Person>> GetStudentsByLessonId(int? lessonId);

        public Task<StudentLesson> GetStudentLesson(int? lessonId, int? personId);
        public Task<StudentLesson> GetStudentLessonById(int studentLessonId);
        public Task<StudentLesson> AddStudentLesson(StudentLesson studentLesson);
        public Task<StudentLesson> DeleteStudentLesson(StudentLesson studentLesson);

        public Task<bool> DeleteStudentLesson(int studentLessonId);

        public Task<bool> DeleteAllStudentLessons(int? personId);
    }
}