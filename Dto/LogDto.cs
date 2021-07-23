using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class LogDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Data { get; set; }
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
    }
    public static class LogDtoExtension
    {
        public static Log ToModel(this LogDto dto)
        {
            return new Log
            {
                Id = dto.Id,
                CreationDate = dto.CreationDate,
                Data = dto.Data,
                Person = dto.Person?.ToModel(),
                PersonId = dto.PersonId
            };
        }
    }

    public static class LogExtension
    {
        public static LogDto ToDto(this Log model)
        {
            return new LogDto
            {
                Id = model.Id,
                CreationDate = model.CreationDate,
                Data = model.Data,
                Person = model.Person?.ToDto(),
                PersonId = model.PersonId
            };
        }
    }
}
