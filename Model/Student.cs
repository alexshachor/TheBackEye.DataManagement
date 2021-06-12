using System;

namespace Model
{
    public class Student
    {
        public int Id { get; set; }
        public string BirthId { get; set; }
        public DateTime DateBirth { get; set; }
        public Person Person { get; set; }
        public int PersonId { get; set; }
    }
}
