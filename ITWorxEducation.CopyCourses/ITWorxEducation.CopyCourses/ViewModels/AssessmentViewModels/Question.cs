using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    public class Question :QuestionBase
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
}
