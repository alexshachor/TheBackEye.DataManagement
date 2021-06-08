using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class StudentClassDto
    {
        public int Id { get; set; }
        public GradeDto Grade { get; set; }
        public int ClassId { get; set; }
        public StudentDto Student { get; set; }
        public int StudentId { get; set; }
    }
    public static class StudentClassDtoExtension
    {
        public static StudentClass ToModel(this StudentClassDto dto)
        {
            return new StudentClass
            {
                Id = dto.Id,
                ClassId = dto.ClassId,
                StudentId = dto.StudentId,
                Student = dto.Student.ToModel(),
                Grade = dto.Grade.ToModel()
            };
        }
    }
}
