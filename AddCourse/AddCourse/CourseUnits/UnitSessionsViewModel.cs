using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.CourseUnits
{
    class UnitSessionsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public int Duration { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSessionsLockedInCourse { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool IsRemovedFromCoursePlan { get; set; }
        public Guid? PushedFromCoursePlannerSessionId { get; set; }
        public Guid UnitId { get; set; }
        public int? Lectures { get; set; }
        public int? Weeks { get; set; }
        public List<SessionMaterialBasicInfoViewModel> Materials { get; set; }
    }
}
