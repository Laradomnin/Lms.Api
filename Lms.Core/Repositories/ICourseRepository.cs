using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface  ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();

        Task<Course> GetCourse(int id);
        Task<Course> FindAsync(int id);
        Task<bool> AnyAsync(int? id);
        Task AddAsync(Course course);
        void Update(Course course);
        void Remove(Course course);
        Task <Course> GetCourse(string title);
        Task<IEnumerable<Course>> GetAsync(bool includeModules);
    }
}
