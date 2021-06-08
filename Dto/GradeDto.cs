using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
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

            };
        }
    }
}
