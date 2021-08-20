using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class StudentLessonDto
    {
        public int Id { get; set; }
        public int? LessonId { get; set; }
        public int? PersonId { get; set; }
    }

    public static class StudentClassDtoExtension
    {
        public static StudentLesson ToModel(this StudentLessonDto dto)
        {
            return new StudentLesson
            {
                Id = dto.Id,
                LessonId = dto.LessonId,
                PersonId = dto.PersonId
            };
        }
    }

    public static class StudentClassExtension
    {
        public static StudentLessonDto ToDto(this StudentLesson model)
        {
            return new StudentLessonDto
            {
                Id = model.Id,
                LessonId = model.LessonId,
                PersonId = model.PersonId
            };
        }
    }
}
