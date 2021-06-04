using System;

namespace Model
{
    public class Teachers
    {
        public int Id { get; set; }
        public int Password { get; set; }
        public Persons Person { get; set; }
        public int PersonId { get; set; }
        public Schools School { get; set; }
        public int SchoolId { get; set; }
    }
}