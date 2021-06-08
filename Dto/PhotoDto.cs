using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public StudentDto Student { get; set; }
        public int StudentId { get; set; }
        public byte[] Data { get; set; }
    }
}
