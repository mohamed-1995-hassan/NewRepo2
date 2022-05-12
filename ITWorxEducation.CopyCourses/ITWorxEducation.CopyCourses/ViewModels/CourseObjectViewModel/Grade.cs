using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.CourseObjectViewModel
{
    public class Grade
    {
        public string Name { get; set; }
        public string ContextId { get; set; }
        public int? ContextTypeId { get; set; }
        public int InstituteTypeId { get; set; }
        public bool IsActive { get; set; }
        public string TranslationKey { get; set; }
        public string UniqueIdentifier { get; set; }
        public int Order { get; set; }
        public string ExternalId { get; set; }
        public int Id { get; set; }
    }
}
