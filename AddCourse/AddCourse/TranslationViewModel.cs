using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse
{
    public class TranslationViewModel
    {
        public TranslationViewModel() { }
        public TranslationViewModel(string propertyValue, Language language, string entityId, string entityType)
        {
            this.PropertyValue = propertyValue;
            this.Language = language;
            this.EntityId = entityId;
            this.EntityType = entityType;
        }
        public string PropertyValue { get; set; }
        public Language Language { get; set; }
        public string EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
