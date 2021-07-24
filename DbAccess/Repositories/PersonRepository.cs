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
        public PersonRepository(BackEyeContext context, ILogger<PersonRepository> logger, ILessonRepository lessonRepository, IStudentLessonRepository studentLessonRepository, IMeasurementRepository measurementRepository)
        {
            _context = context;
            _logger = logger;
            _lessonRepository = lessonRepository;
            _studentLessonRepository = studentLessonRepository;
            _measurementRepository = measurementRepository;
        }



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

        public async Task<bool> DeletePerson(int personId)
        {
            try
            {
                var student = await GetPerson(personId);

                if (student == null)
                {
                    throw new Exception($"Person id: {personId} not found in DB");
                }
                //TODO: in case of teacher should we delete lesson too?
                await _measurementRepository.DeleteAllStudentMeasurement(personId);
                await _studentLessonRepository.DeleteAllStudentLessons(personId);

                _context.Persons.Remove(student);
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
