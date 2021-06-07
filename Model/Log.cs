using System;

namespace Model
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Data { get; set; }
        public Person Person { get; set; }
        public int PersonId { get; set; }
    }
}