using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.CourseObjectViewModel
{
    public class ContextCustomFieldsViewModel
    {
        public int Id { get; set; }
        public int ContextCustomFieldId { get; set; }
        public int CustomFieldId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? NumberValue { get; set; }
        public DateTime? DateValue { get; set; }
        public List<int> ListValueIds { get; set; }
        public List<CustomValuesViewModel> ListCustomFieldValues { get; set; }
        public bool IsRequired { get; set; }
        public CustomFieldType Type { get; set; }
    }
}
