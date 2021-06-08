using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public int Password { get; set; }
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
        public SchoolDto School { get; set; }
        public int SchoolId { get; set; }
    }
    public static class TeacherDtoExtension
    {
        public static Teacher ToModel(this TeacherDto dto)
        {
            return new Teacher
            {

            };
        }
    }
}
