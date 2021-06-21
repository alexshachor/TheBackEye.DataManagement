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

        public async Task<Person> AddPerson(Person person)
        {
            try
            {
                var personFromDb = await GetPersonByEmailPasswordId(person.Email, person.Password, person.BirthId);
                if (personFromDb == null)
                {
                    _context.Add(person);
                    await _context.SaveChangesAsync();
                    return person;
                }
                _logger.LogInformation($"Person with birth id: {personFromDb.BirthId} already exist");
                return personFromDb;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot add preson to DB. due to: {e}");
                return null;
            }
        }

        private async Task<Person> GetPersonByEmailPasswordId(string email, string password, string birthId)
        {
            try
            {
                return await _context.Persons.Where(x => x.Email == email && x.Password == password && x.BirthId == birthId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get person from DB. Due to: {e}");
                return null;
            }
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return person;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot update person in DB. person id: {person.Id}. due to: {e}");
                return null;
            }
        }
    }
}
