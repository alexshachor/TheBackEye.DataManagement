using System;

namespace Model
{
    public class Students
    {
        public int Id { get; set }
        public int IdNumber { get; set }
        public DateTime DateBirth { get; set }
        public Persons Persons { get; set }
        public int PersonId { get; set }
    }
}
