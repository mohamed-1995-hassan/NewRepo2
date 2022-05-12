using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.CourseObjectViewModel
{
    public class Language
    {
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public string CultureName { get; set; }
        public string DisplayName { get; set; }
        public CultureInfo Culture{ get; private set; }
    }
}
