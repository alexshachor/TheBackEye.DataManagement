using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class LogDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Data { get; set; }
        public PersonDto Person { get; set; }
        public int PersonId { get; set; }
    }
}
