using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class StudentDto
    {
        public int Id { get; set; }
        public int IdNumber { get; set; }
        public DateTime DateBirth { get; set; }
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
    }
    public static class StudentDtoExtension
    {
        public static Student ToModel(this StudentDto dto)
        {
            return new Student
            {
                Id = dto.Id,
                IdNumber = dto.IdNumber,
                DateBirth = dto.DateBirth,
                PersonId = dto.PersonId,
                Person = dto.Person.ToModel()
            };
        }
    }
}
