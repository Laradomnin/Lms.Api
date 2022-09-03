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
            await db.SaveChangesAsync();
        }
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

        //Från booking app____________________________

        //private static IEnumerable<Course> GetCourses()
        //{
        //    var faker = new Faker("sv");

        //    var courses = new List<Course>();

        //    for (int i = 0; i < 20; i++)
        //    {
        //        var temp = new Course ();
        //        {
        //            string Title = faker.Company.CatchPhrase();

        //            DateTime StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5));
        //        };

        //        courses.Add(temp);
        //    }

        //    return courses;
        //}
    }
}
