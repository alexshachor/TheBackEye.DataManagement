using System;

namespace Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public int Password { get; set; }
        public Person Person { get; set; }
        public int PersonId { get; set; }
        public School School { get; set; }
        public int SchoolId { get; set; }
    }
}