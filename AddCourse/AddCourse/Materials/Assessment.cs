using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.Materials
{
    class AssessmentElement
    {
        public Assessment Assessment { get; set; }
        public string AssessmentRequestID { get; set; }
        public string EncryptedAuthorizationData { get; set; }
        public string ObjectState { get; set; }
        public object Permissions { get; set; }
        public object Questions { get; set; }
    }
    class Assessment
    {
        public bool AllowExceedingTime { get; set; }
        public bool AllowLateSubmission { get; set; }
        public object AssessmentExternalTool { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public object DueDate { get; set; }
        public object Duration { get; set; }
        public bool EnablePublishingScore { get; set; }
        public string GradableItemId { get; set; }
        public int Grade { get; set; }
        public string GradingMode { get; set; }
        public string GradingPeriodId { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssessmentProctored { get; set; }
        public bool IsGroupAssignees { get; set; }
        public bool IsProject { get; set; }
        public bool IsSpecificAssignees { get; set; }
        public MetaData MetaData { get; set; }
        public int Mode { get; set; }
        public int PassingScore { get; set; }
        public object ProctoredConfiguration { get; set; }
        public Question[] Questions { get; set; }
        public object RandomizationCriteriaData { get; set; }
        public object RubricId { get; set; }
        public object ScormFile { get; set; }
        public string Status { get; set; }
        public string SubmissionType { get; set; }
        public object Time { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

    }

    class AssessmentFiles : Assessment
    {
        public FileOut[] Files { get; set; }
    }

    class FileOut
    {
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string downloadUrl { get; set; }
        public string extension { get; set; }
        public string fileNameWithoutExtention { get; set; }
        public bool isAdded { get; set; }
        public string playUrl { get; set; }
    }
    class MetaData
    {
        public string CourseId { get; set; }
        public string MaterialId { get; set; }
        public string OrganizationId { get; set; }
        public Object SchoolId { get; set; }
        public string SessionId { get; set; }
    }

    class Question
    {
        public object Body { get; set; }
        public object CreateMode { get; set; }
        public string DifficultyLevel { get; set; }
        public string DifficultyLevelId { get; set; }
        public bool? EnableShuffleAnswers { get; set; }
        public int Grade { get; set; }
        public string Hint { get; set; }
        public string IsOccurrenceMessage { get; set; }
        public bool? IsShowHint { get; set; }
        public string ObjectState { get; set; }
        public int Order { get; set; }
        public object PreviewMode { get; set; }
        public string QuestionTypeUniqueName { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string clientId { get; set; }
        public int id { get; set; }
        public object indexforcorrect { get; set; }
    }

    class QuestionFiles : Question
    {
        public string PlayUrl { get; set; }
        public File File { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string DownloadUrl { get; set; }
    }
    class QuestionImage : Question
    {
        public string ImageUrl { get; set; }
        public QuestionImageFile QuestionImageFile { get; set; }
    }

    class QuestionImageFile
    {
        public object Assessment { get; } = null;
        public int AssessmentId { get; set; } = 0;
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
    }

    class Body : BodyBase
    {
        public Options[] Options { get; set; }
    }
    class Options
    {
        public int Id { get; set; }
        public bool IsCorrect { get; set; }
        public string Title { get; set; }
    }
    class FixedOptions
    {
        public string Title { get; set; }
    }
    class MatchingOrderOptions : Options
    {
        public int Order { get; set; }
    }
    interface BodyBase
    { }
    class MatchingBody : BodyBase
    {
        public FixedOptions[] FixedOptions { get; set; }
        public MatchingOrderOptions[] Options { get; set; }
    }
    class BodyOrder : BodyBase
    {
        public MatchingOrderOptions[] Options { get; set; }
    }

    class FormulaBody : BodyBase
    {
        public string ErrorMargin { get; set; }
        public string Formula { get; set; }
        public string FormulaDecimalPoint { get; set; }
        public Variables[] Variables { get; set; }
    }
    class Variables
    {
        public string Dataset { get; set; }
        public string Name { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }
        public int VariableDataType { get; set; }
        public int exampleValue { get; set; }
    }

    class BlankBody : BodyBase
    {
        public BlankAnswers[] BlankAnswers { get; set; }
        
    }
    class BlankAnswers
    {
        public MatchingOrderOptions[] Options { get; set; }
        public int Order { get; set; }
        public string tempOption { get; set; }
    }

    class File
    {
        public string ContentType { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
    }
}
