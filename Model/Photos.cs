using System;
using System.Collections.Generic;

namespace Model
{
    public class Photos
    {
        public int Id { get; set; }
        public Students Student { get; set; }
        public int StudentId { get; set; }
        public List<byte[]> Data { get; set; }
    }
}