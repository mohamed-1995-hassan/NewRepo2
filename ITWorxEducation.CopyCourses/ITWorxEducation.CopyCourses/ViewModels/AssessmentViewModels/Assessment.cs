using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    public class Assessment
    {
        public bool AllowExceedingTime { get; set; }
        public bool AllowLateSubmission { get; set; }
        public object AssessmentExternalTool { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public object DueDate { get; set; }
        public object Duration { get; set; }
        public bool? EnablePublishingScore { get; set; }
        public string GradableItemId { get; set; }
        public int Grade { get; set; }
        public string GradingMode { get; set; }
        public string GradingPeriodId { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAssessmentProctored { get; set; }
        public bool IsGroupAssignees { get; set; }
        public bool IsSpecificAssignees { get; set; }
        public MetaData MetaData { get; set; }
        public int Mode { get; set; }
        public int PassingScore { get; set; }
        public object ProctoredConfiguration { get; set; }
        public QuestionBase[] Questions { get; set; }
        public object RandomizationCriteriaData { get; set; }
        public object RubricId { get; set; }
        public object ScormFile { get; set; }
        public string Status { get; set; }
        public string SubmissionType { get; set; }
        public object Time { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
