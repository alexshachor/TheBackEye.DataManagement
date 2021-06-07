using System;

namespace Model
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
    }
}