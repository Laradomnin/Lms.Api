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

            var courses = GenerateCourses(10);
            await db.Course.AddRangeAsync(courses);

            //var modules = GenerateModules(5, courses);
            //await db.Module.AddRangeAsync(modules);

            await db.SaveChangesAsync();
        }

        private static IEnumerable<Course> GenerateCourses(int numberOfCourses)
        {
            var courses = new List<Course>();

            for (int i = 0; i < numberOfCourses; i++)
            {

                var modules = new List<Module>();

                for (int x = 0; x < 5; x++)
                {
                    var ttitle = faker.Company.CatchPhrase();
                    var sstartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5));
                    var module = new Module(ttitle, sstartDate);
                    modules.Add(module);
                }

                var title = faker.Company.CatchPhrase();
                DateTime startDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5));
                var course = new Course(title, startDate)
                {
                    Modules = modules
                };




                courses.Add(course);
            }
            return courses;
        }
        //private static IEnumerable<Module> GenerateModules(int numberOfModules, IEnumerable<Course> courses)
        //{
        //    var modules = new List<Module>();
        //    for (int i = 0; i < numberOfModules; i++)
        //    {
        //        var title = faker.Company.CatchPhrase();
        //        DateTime startDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5));
        //        modules.Add(new Module(title, startDate)
        //        {
        //            CourseId = courses.First().Id
        //        });
        //    }
        //    return modules;
        //}
    }
}
