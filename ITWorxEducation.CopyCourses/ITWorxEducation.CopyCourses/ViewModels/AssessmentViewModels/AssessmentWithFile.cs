using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    public class AssessmentWithFile : Assessment
    {
        public AssessmentFiles[] Files { get; set; }
    }
}
