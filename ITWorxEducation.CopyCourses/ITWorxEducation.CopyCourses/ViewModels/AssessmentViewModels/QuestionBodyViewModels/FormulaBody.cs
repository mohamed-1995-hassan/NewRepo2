using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels.QuestionBodyViewModels
{
    public class FormulaBody
    {
        public string ErrorMargin { get; set; }
        public string Formula { get; set; }
        public string FormulaDecimalPoint { get; set; }
        public Variables[] Variables { get; set; }
    }
}
