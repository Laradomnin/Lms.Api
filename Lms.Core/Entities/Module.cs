using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
#nullable disable
    public class Module
    {

        public Module(string title, DateTime startDate)
        {
            Title = title;
            StartDate = startDate;
        }

        public int Id { get; set; } 
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public int CourseId { get; set; }

    }
}
