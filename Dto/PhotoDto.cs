using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public StudentDto Student { get; set; }
        public int StudentId { get; set; }
        public byte[] Data { get; set; }
    }
    public static class PhotoDtoExtension
    {
        public static Photo ToModel(this PhotoDto dto)
        {
            return new Photo
            {
                Id = dto.Id,
                StudentId = dto.StudentId,
                Student = dto.Student.ToModel(),
                Data = dto.Data
            };
        }
    }
}
