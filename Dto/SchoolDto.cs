using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto
{
    public class SchoolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public static class SchoolDtoExtension
    {
        public static School ToModel(this SchoolDto dto)
        {
            return new School
            {

            };
        }
    }
}
