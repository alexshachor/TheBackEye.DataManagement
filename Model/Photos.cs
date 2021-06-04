using System;
using System.Collections.Generic;

namespace Model
{
    public class Photos
    {
        public int Id { get; set; }
        public Students Student { get; set; }
        public int StudentId { get; set; }
        // TODO - ask if it need to be a list or singel photo 
        public List<byte[]> Data { get; set; }
    }
}