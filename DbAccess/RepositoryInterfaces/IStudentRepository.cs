//using Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DbAccess.RepositoryInterfaces
//{
//    public interface IStudentRepository
//    {
//        public Task<bool> IsStudentExist(int studentId);
//        public Task<bool> IsStudentExistByBirthId(string birthId);
//        public Task<Student> AddStudent(Student student);
//        public Task<Student> UpdateStudent(Student student);

//        public Task<bool> DeleteStudentById(int studentId);
//        public Task<Student> GetStudentById(int studentId);
//        public Task<Student> GetStudentByBirthId(string studentBirthId);
//        public Task<List<Student>> GetStudentsByClassId(int classId);
//    }
//}
