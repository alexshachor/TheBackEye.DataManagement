using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface IPersonRepository
    {
        public Task<Person> GetPersonByPassword(string password);
        public Task<Person> GetPersonByEmailPassword(string email, string password);
        public Task<Person> AddPerson(Person person);
        public Task<Person> UpdatePerson(Person person);
    }
}
