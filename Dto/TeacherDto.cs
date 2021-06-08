using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
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
                Id = dto.Id,
                PersonId = dto.PersonId,
                Person = dto.Person.ToModel(),
                Password = dto.Password,
                SchoolId = dto.SchoolId,
                School = dto.School.ToModel()
            };
        }
    }
}
