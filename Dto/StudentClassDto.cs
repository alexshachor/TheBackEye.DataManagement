using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class StudentClassDto
    {
        public int Id { get; set; }
        public GradeDto Grade { get; set; }
        public int ClassId { get; set; }
        public StudentDto Student { get; set; }
        public int StudentId { get; set; }
    }
}
