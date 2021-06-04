using System;

namespace Model
{
    public class Logs
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Data { get; set; }
        public Persons Person { get; set; }
        public int PersonId { get; set; }
    }
}