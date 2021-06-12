using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string BirthId { get; set; }
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
                BirthId = dto.BirthId,
                DateBirth = dto.DateBirth,
                PersonId = dto.PersonId,
                Person = dto.Person.ToModel()
            };
        }
    }

    public static class StudentExtension
    {
        public static StudentDto ToDto(this Student model)
        {
            return new StudentDto
            {
                Id = model.Id,
                BirthId = model.BirthId,
                DateBirth = model.DateBirth,
                PersonId = model.PersonId,
                Person = model.Person.ToDto()
            };
        }
    }
}
