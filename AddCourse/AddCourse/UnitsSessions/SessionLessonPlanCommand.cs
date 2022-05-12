using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.UnitsSessions
{
    class SessionLessonPlanCommand
    {
        public int CourseId { get; set; }
        public Guid? SessionId { get; set; }
        public Guid TemplateId { get; set; }
        public string Body { get; set; }
    }
}
