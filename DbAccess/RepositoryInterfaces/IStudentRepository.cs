using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface IStudentRepository
    {
        public Task<bool> IsStudentExist(int studentId);
        public Task<Student> AddStudent(Student student);
        public Task<Student> UpdateStudent(Student student);

        public Task<Student> DeleteStudentById(int studentId);
        public Task<Student> GetStudentById(int studentId);
        public Task<Student> GetStudentByBirthId(int studentBirthId);
        public Task<List<Student>> GetStudentsByClassId(int classId);
    }
}
