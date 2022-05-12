using ITWorxEducation.CopyCourses.Helpers;
using ITWorxEducation.CopyCourses.Helpers.JsonConverterManagment;
using ITWorxEducation.CopyCourses.IServices;
using ITWorxEducation.CopyCourses.ViewModels.CourseObjectViewModel;
using ITWorxEducation.CopyCourses.ViewModels.CoursesViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ITWorxEducation.CopyCourses.Services
{
    public class WinjiCourses : IWinjiCourses
    {
        IConfiguration configuration = Program.LoadConfiguration();
        JArray CourseRounds = new JArray();
        public string TokenSender { get; set; }
        public string TokenReceiver { get; set; }
        private readonly ICallBackRestApi _callBackRestApi;
        private readonly IWinjiRounds _winjiRounds;

        public WinjiCourses(ICallBackRestApi callBackRestApi,IWinjiRounds winjiRounds)
        {
            _callBackRestApi = callBackRestApi;
            _winjiRounds = winjiRounds;
        }
        public void GetWinjiCourses(string resultTokenSender, string resultTokenReceiver)
        {
            TokenSender = resultTokenSender;
            TokenReceiver = resultTokenReceiver;
            GetUnPinnedCourse();
        }
        public void GetUnPinnedCourse()
        {
            string unpinned = "{\"CoursesCount\": null,\"CustomFieldsSearchCrieteria\":[],\"GetArchivedCourses\": false,\"OrganizationTermId\": null,\"courseKeyWord\": \"\",\"courseStatus\": null,\"getPendingRequestsCount\": true,\"instructorId\": null,\"orderBy\": \"0\",\"pageNumber\": 1,\"pinStatus\": null}";
            string result = _callBackRestApi.CallAPi($"{configuration["AuthorizationTokenSource:BaseURL"]}/api/CourseApi/GetSchoolCoursesList", HttpMethod.Post, TokenSender, unpinned);
            CourseWrapper courseWrapper = JsonManagment.DeserializeJson<CourseWrapper>(result);
            for (int i = 0; i < courseWrapper.UnPinnedCourses.Count; i++)
            {
                Console.WriteLine(courseWrapper.UnPinnedCourses[i].Id);
            }
            for (int courseNumber = 0; courseNumber < courseWrapper.UnPinnedCourses.Count;courseNumber++)
            {
                if (courseWrapper.UnPinnedCourses[courseNumber].Id == "83838" || courseWrapper.UnPinnedCourses[courseNumber].Id == "83837" || courseWrapper.UnPinnedCourses[courseNumber].Id == "83700" || courseWrapper.UnPinnedCourses[courseNumber].Id == "83684" || courseWrapper.UnPinnedCourses[courseNumber].Id == "31298")
                    continue;
                Course course = GetCourseById(courseWrapper.UnPinnedCourses[courseNumber].Id, TokenSender);
               //Console.WriteLine(course.OnBehalfTeacherId);
                AddNewCourse(course);
               
            }
        }
        

        public Course GetCourseById(string CourseId,string token)
        {
            string result = _callBackRestApi.CallAPi($"{configuration["AuthorizationTokenSource:BaseURL"]}/api/CourseApi/GetCourseDetailsAndUpdateLastAccess?courseGroupId={CourseId}", HttpMethod.Get, token);
            JObject JCourse = JObject.Parse(result);
            Console.WriteLine(JCourse);
            string Rounds = JsonConvert.SerializeObject(JCourse["Courses"]);
            CourseRounds = JArray.Parse(Rounds);
            Course courseObject = JsonManagment.DeserializeJson<Course>(JCourse["Course"].ToString());
            Console.WriteLine(JCourse["Course"]["CourseGrades"].ToString());
            List<Grade> courseGradesList = JsonManagment.DeserializeJson<List<Grade>>(JCourse["Course"]["CourseGrades"].ToString());
            ManageGrades(courseObject,courseGradesList);
            courseObject.Status = (int)JCourse["Status"];
            courseObject.CourseImageId = null;
            if(courseObject.Type == 0)
            {
                string adminId = configuration["TeacherEmail"];
                courseObject.OnBehalfTeacherId = int.Parse(adminId);
            }
            else if(courseObject.Type == 1)
            {
                courseObject.OnBehalfTeacherId = null;
            }
            return courseObject;
        }
        public void AddNewCourse(Course course)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenReceiver);
            string JsonSerializeForAddCourseGroup = JsonConvert.SerializeObject(course);
            var stringContentUnPinnedCourses = new StringContent(JsonSerializeForAddCourseGroup, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessageForAddingCourseGroup = httpClient.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/CourseApi/AddCourseGroup", stringContentUnPinnedCourses).Result;
            var JsonForAddingCourseGroup = JObject.Parse(httpResponseMessageForAddingCourseGroup.Content.ReadAsStringAsync().Result);
            //start Round Adding And Retriving cycle
            ManageRounds(CourseRounds, JsonForAddingCourseGroup, course.OnBehalfTeacherId);
        }
        public void ManageGrades(Course course, List<Grade> courseGradesList)
        {
            int maxGrade = 0, minGrade = 0, gradeId = 0;
            if (courseGradesList.Count > 1)
            {
                minGrade = courseGradesList[0].Id;
                maxGrade = courseGradesList[courseGradesList.Count - 1].Id;
            }
            else
            {
                gradeId = courseGradesList[0].Id;
            }
            course.MaxGrade = maxGrade;
            course.MinGrade = minGrade;
            course.GradeId = gradeId;
        }
        public void ManageRounds(JArray Rounds,JObject AddedCourse, int? adminId)
        {
            _winjiRounds.StartAddingRounds(Rounds, AddedCourse,TokenSender,TokenReceiver , adminId);
        }
    }
}
