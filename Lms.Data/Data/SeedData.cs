using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data
{
    public class SeedData
    {
        //Från LexiconUniversity__________________________

        private static Faker faker = null;
        public static async Task InitAsync(LmsApiContext db)
        {
            if (await db.Course.AnyAsync()) return;
            faker = new Faker("sv");
            //var modules = GenerateModules(10);
            //await db.Module.AddRangeAsync(modules);

            var courses = GenerateCourses(10);
            await db.Course.AddRangeAsync(courses);

            await db.SaveChangesAsync();
        }
        //private static IEnumerable<Module> GenerateModules(int numberOfModules)
        //{
        //    var modules = new List<Module>();
        //    for (int i = 0; i < numberOfModules; i++)
        //    {
        //        var title = faker.Company.CatchPhrase();
        //        DateTime startDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5));
        //        modules.Add(new Module(title, startDate));
        //    }
        //    return modules;
        //}
        private static IEnumerable<Course> GenerateCourses(int numberOfCourses)
        {
            var courses = new List<Course>();
            for (int i = 0; i < numberOfCourses; i++)
            {
                var title = faker.Company.CatchPhrase();
                DateTime startDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5));

                courses.Add(new Course(title, startDate));
            }
            return courses;
        }
    }
}