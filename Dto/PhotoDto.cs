using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
        public byte[] Data { get; set; }
    }
    public static class PhotoDtoExtension
    {
        public static Photo ToModel(this PhotoDto dto)
        {
            return new Photo
            {
                Id = dto.Id,
                PersonId = dto.PersonId,
                Person = dto.Person.ToModel(),
                Data = dto.Data
            };
        }
    }

    public static class PhotoExtension
    {
        public static PhotoDto ToDto(this Photo model)
        {
            return new PhotoDto
            {
                Id = model.Id,
                Data = model.Data,
                PersonId = model.PersonId,
                Person = model.Person.ToDto(),
            };
        }
    }
}
