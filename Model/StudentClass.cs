using System;

namespace Model
{
    public class StudentClass
    {
        public int Id { get; set; }
        public Grade Grade { get; set; }
        public int ClassId { get; set; }
        public Student Student { get; set; }
        public int StudentId { get; set; }
    }
}
