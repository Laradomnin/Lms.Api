using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{ 
    #nullable disable
    public class CourseDto
    {
        //public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<ModuleDto> Modules { get; set; }

    }
}
