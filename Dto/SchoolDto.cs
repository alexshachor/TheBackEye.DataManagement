using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class SchoolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public static class SchoolDtoExtension
    {
        public static School ToModel(this SchoolDto dto)
        {
            return new School
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email
            };
        }
    }

    public static class SchoolExtension
    {
        public static SchoolDto ToDto(this School model)
        {
            return new SchoolDto
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email
            };
        }
    }
}
