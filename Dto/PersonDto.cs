using DataAccess.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string BirthId { get; set; }
        public string Password { get; set; }
        public PersonType Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
    public static class PersonDtoExtension
    {
        public static Person ToModel(this PersonDto dto)
        {
            return new Person
            {
                Id = dto.Id,
                BirthId = dto.BirthId,
                Password = dto.Password,
                Type = dto.Type,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Token = dto.Token
            };
        }
    }
    public static class PersonExtension
    {
        public static PersonDto ToDto(this Person model)
        {
            return new PersonDto
            {
                Id = model.Id,
                BirthId = model.BirthId,
                Password = model.Password,
                Type = model.Type,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Token = model.Token
            };
        }
    }
}
