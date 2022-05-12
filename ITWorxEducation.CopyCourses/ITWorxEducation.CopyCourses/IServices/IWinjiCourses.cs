using ITWorxEducation.CopyCourses.ViewModels.CourseObjectViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices
{
    public interface IWinjiCourses
    {
        public void GetWinjiCourses(string resultTokenSender, string resultTokenReceiver);
        public void GetUnPinnedCourse();
        public Course GetCourseById(string CourseId, string token);
        public void AddNewCourse(Course course);
        public void ManageGrades(Course course, List<Grade> courseGradesList);
        public void ManageRounds(JArray Rounds, JObject AddedCourse, int? adminId);
    }
}
