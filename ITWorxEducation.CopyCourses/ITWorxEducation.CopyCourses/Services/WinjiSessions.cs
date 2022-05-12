using ITWorxEducation.CopyCourses.FileManagment;
using ITWorxEducation.CopyCourses.Helpers;
using ITWorxEducation.CopyCourses.IServices;
using ITWorxEducation.CopyCourses.IServices.IMaterial;
using ITWorxEducation.CopyCourses.Services.Material;
using ITWorxEducation.CopyCourses.ViewModels.SessionViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Services
{
    public class WinjiSessions:IWinjiSessions
    {
        IConfiguration configuration = Program.LoadConfiguration();
        private readonly ICallBackRestApi _callBackRestApi;
        private readonly IMaterialManagment _materialManagment;
        public string _selectedRound { get; set; }
        public string _roundId { get; set; }
        public string _unitId { get; set; }
        public string _tokenSender { get; set; }
        public string _tokenReceiver { get; set; }
        public string _unitIdAdd { get; set; }
        public WinjiSessions(ICallBackRestApi callBackRestApi, IMaterialManagment materialManagment)
        {
            _callBackRestApi = callBackRestApi;
            _materialManagment = materialManagment;
        }
        public void ManageSessions(string roundId, string selectedRound,string unitIdAdd , string unitId, string tokenSender, string tokenReceiver)
        {
            _roundId = roundId;
            _selectedRound = selectedRound;
            _unitId = unitId;
            _tokenSender = tokenSender;
            _tokenReceiver = tokenReceiver;
            _unitIdAdd = unitIdAdd;
            ReadAllSessions();
        }

        public void ReadAllSessions()
        {
            string link = $"{configuration["AuthorizationTokenSource:BaseURL"]}/api/CourseApi/GetSessionsByUnitId?courseId=" + int.Parse(_selectedRound) + "&unitId=" + _unitId;
            string session = _callBackRestApi.CallAPi(link,HttpMethod.Get,_tokenSender);
            JArray selectedSessionList = JArray.Parse(session);
            ReadSessionDetails(selectedSessionList);
        }

        public void ReadSessionDetails(JArray selectedSessionList)
        {
            for (int sessionNumber = 0; sessionNumber < selectedSessionList.Count; sessionNumber++)
            {
                string link = $"{configuration["AuthorizationTokenSource:BaseURL"]}/api/CourseApi/GetSessionDetails?pageNumber=1&pageSize=15&sessionId=" + selectedSessionList[sessionNumber]["Id"];
                string sessionDetails = _callBackRestApi.CallAPi(link, HttpMethod.Get, _tokenSender);
                JObject sessionInfo = JObject.Parse(sessionDetails);
                PrepareSessionObject(sessionInfo, selectedSessionList, sessionNumber);
            }
        }
        
        public void PrepareSessionObject(JObject sessionInfo, JArray selectedSessionList,int sessionNumber)
        {
            SessionWithoutFile sessionObject = null;
            if(!string.IsNullOrEmpty((string)sessionInfo["ObjectiveFileName"]))
            {
                string objectiveFileName = (string)sessionInfo["ObjectiveFileName"];
                string objectiveFileId = (string)sessionInfo["Session"]["ObjectiveFileId"];
                string fileExtention = objectiveFileName.Substring(objectiveFileName.LastIndexOf('.') + 1, (objectiveFileName.Length - (objectiveFileName.LastIndexOf('.') + 1)));
                string Id = ReturnNewFileId(objectiveFileName, objectiveFileId, fileExtention);
                sessionObject = new SessionObject
                {
                    ContextId = _roundId,
                    Duration = selectedSessionList[sessionNumber]["Duration"],
                    Title = (string)selectedSessionList[sessionNumber]["Title"],
                    Objectives = sessionInfo["Session"]["Objectives"],
                    UnitId = _unitIdAdd,
                    Type = (int)sessionInfo["Session"]["Type"],
                    ObjectiveFileId = Id
                };

            }
            else
            {
                sessionObject = new SessionWithoutFile
                {
                    ContextId = _roundId,
                    Duration = selectedSessionList[sessionNumber]["Duration"],
                    Title = (string)selectedSessionList[sessionNumber]["Title"],
                    Objectives = sessionInfo["Session"]["Objectives"],
                    UnitId = _unitIdAdd,
                    Type = (int)sessionInfo["Session"]["Type"],
                };
            }

            AddSession(sessionObject, sessionInfo);

        }

        public string ReturnNewFileId(string objectiveFileName,string objectiveFileId,string fileExtention)
        {
            string fileResult = FileOperations.FileManagment(objectiveFileId, objectiveFileName, fileExtention, int.Parse(_roundId).ToString(), "2", _tokenReceiver, _tokenSender);
            JObject jobjectFileResult = JObject.Parse(fileResult);
            return jobjectFileResult["fileId"].ToString();
        }
        public void AddSession(SessionWithoutFile sessionObject,JObject sessionInfo)
        {
            string sessionBody = JsonConvert.SerializeObject(sessionObject);
            Console.WriteLine(sessionBody);
            var stringContentForAddingCourseSession = new StringContent(sessionBody, UnicodeEncoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenReceiver);
            HttpResponseMessage httpResponseMessageForAddingCourseGroup = httpClient.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/CourseApi/AddSession", stringContentForAddingCourseSession).Result;
            string result = httpResponseMessageForAddingCourseGroup.Content.ReadAsStringAsync().Result;
            SessionId sessionId = JsonConvert.DeserializeObject<SessionId>(result);
            JObject jsonValueForTheLesson = JObject.Parse(result);
            Console.WriteLine(jsonValueForTheLesson);
            _materialManagment.ManageAllMaterial(sessionInfo, sessionId.Id, _roundId, _tokenReceiver, _tokenSender);
        }
    }
    class SessionId
    {
        public string Id { get; set; }
    }
}
