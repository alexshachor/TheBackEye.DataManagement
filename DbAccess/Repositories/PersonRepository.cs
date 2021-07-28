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
        private readonly ILessonRepository _lessonRepository;
        private readonly IStudentLessonRepository _studentLessonRepository;
        private readonly IMeasurementRepository _measurementRepository;
        private readonly ILogRepository _logRepository;

        public PersonRepository(BackEyeContext context, ILogger<PersonRepository> logger, ILessonRepository lessonRepository, IStudentLessonRepository studentLessonRepository, IMeasurementRepository measurementRepository)
        {
            _context = context;
            _logger = logger;
            _lessonRepository = lessonRepository;
            _studentLessonRepository = studentLessonRepository;
            _measurementRepository = measurementRepository;
        }


        /// <summary>
        /// get a person (teacher person) from DB by its given email and password
        /// </summary>
        /// <param name="email">email of the person</param>
        /// <param name="password">hashed password of the person</param>
        /// <returns>the requested person</returns>
        public async Task<Person> GetPersonByEmailPassword(string email, string password)
        {
            try
            {
                return await _context.Persons.Where(x => x.Email == email && x.Password == password && x.Type == DataAccess.Model.PersonType.Teacher).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get teacher from DB. person email: {email}. person password: {password}. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// get a person (student person) from DB by its given password
        /// </summary>
        /// <param name="password">hashed password of the person</param>
        /// <returns>the requested person</returns>
        public async Task<Person> GetPersonByPassword(string password)
        {
            try
            {
                return await _context.Persons.Where(x => x.Password == password && x.Type == DataAccess.Model.PersonType.Student).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get student from DB. person password: {password}. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// add a new person to DB
        /// </summary>
        /// <param name="person">person to add</param>
        /// <returns>the newly added person</returns>
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

        /// <summary>
        /// get any type of person frpm Db by its given email, password and birth id
        /// </summary>
        /// <param name="email">email of the person</param>
        /// <param name="password">password of the person</param>
        /// <param name="birthId">birth id of the person</param>
        /// <returns>the requested person from DB</returns>
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

        /// <summary>
        /// update the person in DB
        /// </summary>
        /// <param name="person">person to update contains the new data</param>
        /// <returns>the newly updated person</returns>
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

        /// <summary>
        /// get a person from DB by its person id
        /// </summary>
        /// <param name="personId">person id</param>
        /// <returns>the requested person from DB</returns>
        public async Task<Person> GetPerson(int personId)
        {
            try
            {
                return await _context.Persons.Where(x => x.Id == personId).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot get erson from DB. person id: {personId}. due to: {e}");
                return null;
            }
        }

        /// <summary>
        /// delete the given person (by its id) from DB
        /// </summary>
        /// <param name="personId">person id</param>
        /// <returns>true if deletion was a success and false otherwise</returns>
        public async Task<bool> DeletePerson(int personId)
        {
            try
            {
                var person = await GetPerson(personId);

                if (person == null)
                {
                    throw new Exception($"Person id: {personId} not found in DB");
                }
                //TODO: in case of teacher should we delete lesson too?
                await _measurementRepository.DeleteAllStudentMeasurement(personId);
                await _studentLessonRepository.DeleteAllStudentLessons(personId);
                await _logRepository.DeletePersonLogs(personId);

                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Cannot delete person from DB. person id: {personId}. due to: {e}");
            }
            return false;
        }
    }
}
