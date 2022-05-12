using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    class QuestionWithFile : Question
    {
        public string PlayUrl { get; set; }
        public InnerFile File { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string DownloadUrl { get; set; }
    }
}
