using System;

namespace Model
{
    public class StudentClass
    {
        public int Id { get; set; }
        public Classes Class { get; set; }
        public int ClassId { get; set; }
        public Students Student { get; set; }
        public int StudentId { get; set; }
    }
}
