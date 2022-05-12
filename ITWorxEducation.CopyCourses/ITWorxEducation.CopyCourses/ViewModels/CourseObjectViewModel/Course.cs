using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.CourseObjectViewModel
{
    public class Course
    {
        public string Name { get; set; }
        public List<TranslationViewModel> NameTranslations { get; set; }
        public int? CourseImageId { get; set; }
        public int? GradeId { get; set; }
        public int? MinGrade { get; set; }
        public int? MaxGrade { get; set; }
        public object SubjectId { get; set; }
        //not mapped
        public int? FeedbackTemplateId { get; set; }
        public string Description { get; set; }
        public TeamChannel MappedTeamChannel { get; set; }
        public int JoiningType { get; set; }
        public bool IsAutoAccept { get; set; }
        public int Type { get; set; }
        public int? MaxCapacity { get; set; }
        public bool IsLearningPathsEnabled { get; set; }
        public string LearningPathTheme { get; set; }
        public bool IsShowLearnersProgress { get; set; }
        public int? OnBehalfTeacherId { get; set; } 
        public int Status { get; set; }
        public bool IsCourseGamificationEnabled { get; set; }
        public bool IsCertificateEnabled { get; set; }
        public string Location { get; set; }
        public bool IsVideoBased { get; set; }
        public int? CompletionCriteria { get; set; }
        public string VideoCoverUrl { get; set; }
        public int? CreditHours { get; set; }
        public int? OriginalCourseCreditHours { get; set; }
        public bool IsSessionsLocked { get; set; }
        public string CourseImageFileId { get; set; }
        public string CourseImageFileSize { get; set; }
        public string CourseImageFileName { get; set; }
        public bool? IsEnableAttendance { get; set; }
        public bool IsEffectivenessLevel1Enabled { get; set; }
        public bool IsEffectivenessLevel2Enabled { get; set; }
        public bool IsProhibitJoinMultipleRounds { get; set; }
        //not mapped
        public List<int> OrganizationTermIds { get; set; }
        //issue line
        public object ResetCourseCount { get; set; }
        public bool IsFinalAssessmentLocked { get; set; }
        public bool IsAssessmentAchievementCoverageEnabled { get; set; }
        public bool IsInClassAchievementCoverageEnabled { get; set; }
        public bool IsQuizAchievementCoverageEnabled { get; set; }
        public bool IsEnableDownloadMaterials { get; set; }
        public bool? IsEffectivenessLevel1Anonymous { get; set; }
        public int? CertificateDuration { get; set; }
        public List<ContextCustomFieldsViewModel> CourseCustomFields { get; set; }
    }
}
