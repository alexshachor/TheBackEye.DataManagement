using DataAccess.Model;
using System;

namespace Model
{
    public class Person
    {
        public int Id { get; set; }
        public string BirthId { get; set; }
        public string Password { get; set; }
        public PersonType Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
