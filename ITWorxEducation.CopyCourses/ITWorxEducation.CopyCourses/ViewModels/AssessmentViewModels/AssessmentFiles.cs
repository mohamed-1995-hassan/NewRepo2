using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    public class AssessmentFiles
    {
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string downloadUrl { get; set; }
        public string extension { get; set; }
        public string fileNameWithoutExtention { get; set; }
        public bool isAdded { get; set; }
        public string playUrl { get; set; }
    }
}
