using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.RoundsViewModels
{
    public class RoundSelfBasedViewModel
    {
        public string CourseGroupId { get; set; }
        public string EndDate { get; set; }
        public string Location { get; set; }
        public int MaxCapacity { get; set; }
        public string StartDate { get; set; }
        public string Title { get; set; }
        public string CopiedFromId { get; set; }
        public List<TitleTranslations> TitleTranslations { get; set; }

    }
}
