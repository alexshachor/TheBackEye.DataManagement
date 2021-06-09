using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class GradeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TeacherDto Teacher { get; set; }
        public int TeacherId { get; set; }
    }
    public static class GradeDtoExtension
    {
        public static Grade ToModel(this GradeDto dto)
        {
            return new Grade
            {
                Id = dto.Id,
                Name = dto.Name,
                Teacher = dto.Teacher.ToModel(),
                TeacherId = dto.TeacherId
            };
        }
    }

    public static class GradeExtension
    {
        public static GradeDto ToDto(this Grade model)
        {
            return new GradeDto
            {
                Id = model.Id,
                Name = model.Name,
                Teacher = model.Teacher.ToDto(),
                TeacherId = model.TeacherId
            };
        }
    }
}
