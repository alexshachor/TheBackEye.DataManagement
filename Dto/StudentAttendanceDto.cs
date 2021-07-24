using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos
{
    public class StudentAttendanceDto
    {
        public PersonDto Person { get; set; }
        public DateTime? EntranceTime { get; set; }
    }
}
