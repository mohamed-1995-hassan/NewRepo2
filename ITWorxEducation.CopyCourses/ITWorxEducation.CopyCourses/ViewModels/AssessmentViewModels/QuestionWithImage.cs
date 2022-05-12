using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    public class QuestionWithImage : Question
    {
        public string ImageUrl { get; set; }
        public QuestionImageFile QuestionImageFile { get; set; }
    }
}
