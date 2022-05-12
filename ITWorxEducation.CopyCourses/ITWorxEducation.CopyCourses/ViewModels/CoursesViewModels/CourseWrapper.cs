using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.CoursesViewModels
{
    class CourseWrapper
    {
        public List<UnPinnedCourse> UnPinnedCourses { get; set; }
        public object PinnedCourses { get; set; }
    }
}
