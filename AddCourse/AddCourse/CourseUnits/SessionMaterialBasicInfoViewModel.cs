using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.CourseUnits
{
    class SessionMaterialBasicInfoViewModel
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int MaterialTypeId { get; private set; }
        public string ContextId { get; protected set; }
        public string ContextType { get; protected set; }
        public string TypeShortName { get; set; }
        public bool IsRemovedFromCoursePlan { get; set; }
        public bool IsUnitCopyingInProgress { get; set; }
        public string Icon { get; set; }
        public List<string> LearningObjectives { get; set; }
        public string Content { get; set; }
    }
}
