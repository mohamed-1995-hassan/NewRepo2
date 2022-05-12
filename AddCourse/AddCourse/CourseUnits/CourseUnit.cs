using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.CourseUnits
{
    class CourseUnit
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public bool IsCourseArchieved { get; set; }
        public Guid? PushedFromCoursePlannerUnitId { get; set; }
        public bool IsRemovedFromCoursePlan { get; set; }
        public List<UnitSessionsViewModel> Sessions { get; set; }
        public int UnitsCount { get; set; }
        public bool IsUnitCopyingInProgress { get; set; }
        public int? TermId { get; set; }
        public string TermName { get; set; }
        public bool IsLastSeenUnit { get; set; }
        public UpcomingSessionMaterial UpcomingSessionMaterial { get; set; }
    }
}
