using System;

namespace Model
{
    public class Student
    {
        public int Id { get; set; }
        public int IdNumber { get; set; }
        public DateTime DateBirth { get; set; }
        public Person Person { get; set; }
        public int PersonId { get; set; }
    }
}
