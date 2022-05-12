using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.UnitsSessions
{
    class SessionAddCommand
    {
        public string Title { get; set; }
        public string Objectives { get; set; }
        public int? Duration { get; set; }
        public int ContextId { get; set; }
        public Guid UnitId { get; set; }
        public DateTime? Date { get; set; }
        public string VoiceNotefileId { get; set; }
        public SessionType? Type { get; set; }
        public string ObjectiveFileId { get; set; }
        public SessionLessonPlanCommand LessonPlanTemplate { get; set; }
        public List<LearningObjectiveCommand> LearningObjectives { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ScheduleActivationDate { get; set; }
    }
}
