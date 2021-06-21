using DbAccess.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly BackEyeContext _context;
        private readonly ILogger _logger;
        public PersonRepository(BackEyeContext context, ILogger<PersonRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Person> GetPersonByPassword(string password)
        {
            try
            {
                return await _context.Persons.Where(x => x.Password == password).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get person from DB. person password: {password}. due to: {e}");
                return null;
            }
        }

        public Task<Person> AddPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public Task<Person> UpdatePerson(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
