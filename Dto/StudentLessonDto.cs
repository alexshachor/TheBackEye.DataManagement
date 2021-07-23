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
        public LessonDto Lesson { get; set; }
        public int LessonId { get; set; }
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
    }

    public static class StudentClassDtoExtension
    {
        public static StudentLesson ToModel(this StudentLessonDto dto)
        {
            return new StudentLesson
            {
                Id = dto.Id,
                Lesson = dto.Lesson?.ToModel(),
                LessonId = dto.LessonId,
                Person = dto.Person?.ToModel(),
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
                Lesson = model.Lesson?.ToDto(),
                LessonId = model.LessonId,
                Person = model.Person?.ToDto(),
                PersonId = model.PersonId
            };
        }
    }
}
