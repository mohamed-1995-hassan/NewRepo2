using ITWorxEducation.CopyCourses.Helpers;
using ITWorxEducation.CopyCourses.IServices;
using ITWorxEducation.CopyCourses.ViewModels.UnitsViewModels;
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
    public class WinjiUnit : IWinjiUnit
    {
        IConfiguration configuration = Program.LoadConfiguration();
        private readonly ICallBackRestApi _callBackRestApi;
        private readonly IWinjiSessions _winjiSessions;
        public int _selectedRound  { get; set; }
        public string _roundId { get; set; }
        public string _tokenSender { get; set; }
        public string _tokenReceiver { get; set; }
        public string _courseId { get; set; }
        public string _roundName { get; set; }

        public WinjiUnit(ICallBackRestApi callBackRestApi,IWinjiSessions winjiSessions)
        {
            _callBackRestApi = callBackRestApi;
            _winjiSessions = winjiSessions;
        }
        public void StartAddingUnits(string roundName,string courseId,int selectedRound,string roundId,string tokenSender,string tokenReceiver)
        {
            _selectedRound = selectedRound;
            _roundId = roundId;
            _tokenSender = tokenSender;
            _tokenReceiver = tokenReceiver;
            _courseId = courseId;
            _roundName = roundName;
            List<UnitViewModel> allModels = ReadAllUnits();
            AddUnitCycle(allModels);
        } 
        public List<UnitViewModel> ReadAllUnits()
        {
           string result = _callBackRestApi.CallAPi($"{configuration["AuthorizationTokenSource:BaseURL"]}/api/CourseUnitApi/GetUnits?contextId={_selectedRound}", HttpMethod.Get,_tokenSender);
            JArray rsultArray = JArray.Parse(result);
            Console.WriteLine(rsultArray);
            List<UnitViewModel> allModels = JsonConvert.DeserializeObject<List<UnitViewModel>>(rsultArray.ToString());
            return allModels;
        }
        public void AddUnitCycle(List<UnitViewModel> unitViewModels)
        {
            for (int unitNumber = 0; unitNumber <unitViewModels.Count;unitNumber++)
            {
                string courseUnitJson = JsonConvert.SerializeObject(new AddUnitViewModel
                {
                    Title = unitViewModels[unitNumber].Title,
                    ContextId = _roundId
                });
                var stringContentForAddingCourseUnit = new StringContent(courseUnitJson, UnicodeEncoding.UTF8,"application/json");
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenReceiver);
                HttpResponseMessage httpResponseMessageForAddingCourseGroup = httpClient.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/CourseUnitApi/AddUnit", stringContentForAddingCourseUnit).Result;
                string result = httpResponseMessageForAddingCourseGroup.Content.ReadAsStringAsync().Result;
                JObject unit = JObject.Parse(result);
                string unitId = unit["Id"].ToString();
                _winjiSessions.ManageSessions(_roundId,_selectedRound.ToString(), unitId, unitViewModels[unitNumber].Id.ToString(), _tokenSender,_tokenReceiver);
            }
        }
    }
}
