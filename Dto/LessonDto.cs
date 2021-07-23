using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class LessonDto
    {
        public int Id { get; set; }
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Platform { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime BreakStart { get; set; }
        public DateTime BreakEnd { get; set; }
        public int MaxLate { get; set; }
        public string ClassCode { get; set; }
    }

    public static class LessonDtoExtension
    {
        public static Lesson ToModel(this LessonDto dto)
        {
            return new Lesson
            {
                Id = dto.Id,
                Person = dto.Person?.ToModel(),
                PersonId = dto.PersonId,
                Name = dto.Name,
                ClassCode = dto.ClassCode,
                BreakStart = dto.BreakStart,
                BreakEnd = dto.BreakEnd,
                DayOfWeek = dto.DayOfWeek,
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsActive = dto.IsActive,
                Link = dto.Link,
                MaxLate = dto.MaxLate,
                Platform = dto.Platform
            };
        }
    }

    public static class LessonExtension
    {
        public static LessonDto ToDto(this Lesson model)
        {
            return new LessonDto
            {
                Id = model.Id,
                Person = model.Person?.ToDto(),
                PersonId = model.PersonId,
                Name = model.Name,
                ClassCode = model.ClassCode,
                BreakStart = model.BreakStart,
                BreakEnd = model.BreakEnd,
                DayOfWeek = model.DayOfWeek,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                IsActive = model.IsActive,
                Link = model.Link,
                MaxLate = model.MaxLate,
                Platform = model.Platform
            };
        }
    }
}
