using System;

namespace Model
{
    public class StudentLesson
    {
        public int Id { get; set; }
        public Lesson Lesson { get; set; }
        public int? LessonId { get; set; }
        public Person Person { get; set; }
        public int? PersonId { get; set; }
    }
}
