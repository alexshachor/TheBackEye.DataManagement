using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class LessonDto
    {
        public int Id { get; set; }
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
        public GradeDto Grade { get; set; }
        public int ClassId { get; set; }
    }

    public static class LessonDtoExtension
    {
        public static Lesson ToModel(this LessonDto dto)
        {
            return new Lesson
            {
                Id = dto.Id,
                Name = dto.Name,
                ClassId = dto.ClassId,
                BreakStart = dto.BreakStart,
                BreakEnd = dto.BreakEnd,
                DayOfWeek = dto.DayOfWeek,
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Grade = dto.Grade.ToModel(),
                IsActive = dto.IsActive,
                Link = dto.Link,
                MaxLate = dto.MaxLate,
                Platform = dto.Platform
            };
        }
    }
}
