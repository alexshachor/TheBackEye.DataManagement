﻿using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.RepositoryInterfaces
{
    public interface ILessonRepository
    {
        public Task<Lesson> GetLesson(string classCode);
        public Task<List<Lesson>> GetLessonsByTeacherId(int teacherId);
        public Task<Lesson> AddLesson(Lesson lesson);
        public Task<Lesson> DeleteLesson(Lesson lesson);
        public Task<Lesson> UpdateLesson(Lesson lesson);
    }
}
