using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LmsApiContext db;
        public CourseRepository (LmsApiContext db)
        { 
            this.db = db;
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await db.Course.ToListAsync();
        }

        public void Add(Course course)
        {
            db.Add(course);
        }

        public Task<bool> AnyAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<Course?> FindAsync(int? id)
        {
            return await db.Course.FirstOrDefaultAsync(c => c.Id == id);
        }   

        public Task<Course> GetCourse(int? id)
        {
            throw new NotImplementedException();
        }

        public void Update(Course course)
        {
            throw new NotImplementedException();
        }
        public void Remove(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
