using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.CourseObjectViewModel
{
    public class TranslationViewModel
    {
        public TranslationViewModel() { }
        public TranslationViewModel(string propertyValue, Language language, string entityId, string entityType)
        {
            PropertyValue = propertyValue;
            Language = language;
            EntityId = entityId;
            EntityType = entityType;
        }
        public string PropertyValue { get; set; }
        public Language Language { get; set; }
        public string EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
