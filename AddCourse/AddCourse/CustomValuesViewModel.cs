using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse
{
    public class CustomValuesViewModel
    {
        public int Id { get; set; }
        public int CustomFieldId { get; set; }
        //The value of the type is 'CustomFieldContextType' enum
        public int ContextType { get; set; }
        public string Value { get; set; }
    }
}
