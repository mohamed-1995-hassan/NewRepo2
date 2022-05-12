using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    public class QuestionImageFile
    {
        public object Assessment { get; } = null;
        public int AssessmentId { get; set; } = 0;
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
    }
}
