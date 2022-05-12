using ITWorxEducation.CopyCourses.Helpers;
using ITWorxEducation.CopyCourses.IServices;
using ITWorxEducation.CopyCourses.ViewModels.RoundsViewModels;
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
    public class WinjiRounds : IWinjiRounds
    {
        IConfiguration configuration = Program.LoadConfiguration();
        private readonly ICallBackRestApi _callBackRestApi;
        private readonly IWinjiUnit _winjiUnit;

        public JArray _Rounds { get; set; }
        public JObject _AddedCourse { get; set; }
        public string _tokenSender { get; set; }
        public string _tokenReceiver { get; set; }
        string AddRound = "";
        JObject JObjectForRound = null;
        public WinjiRounds(ICallBackRestApi callBackRestApi,IWinjiUnit winjiUnit)
        {
            _callBackRestApi = callBackRestApi;
            _winjiUnit = winjiUnit;
        }
        public void StartAddingRounds(JArray Rounds, JObject AddedCourse,string tokenSender,string tokenReceiver, int? adminId)
        {
            _Rounds = Rounds;
            _AddedCourse = AddedCourse;
            _tokenSender = tokenSender;
            _tokenReceiver = tokenReceiver;
            for (int roundNumber = 0 ; roundNumber < Rounds.Count ; roundNumber++)
            {
                RoundItrationForAdding(roundNumber,adminId);
            }
        }
        public void RoundItrationForAdding(int roundNumber,int? adminId) 
        {
            if(_Rounds[roundNumber]["Name"].ToString()!="Original")
            {
                JArray roundArray = JArray.Parse(_Rounds[roundNumber]["TitleTranslations"].ToString());
                List<TitleTranslations> titleTranslations = JsonConvert.DeserializeObject<List<TitleTranslations>>(roundArray.ToString());
                Console.WriteLine(titleTranslations[0].language.CultureName);
                RoundSelfBasedViewModel winjiRounds = null;
                if (adminId == null)
                {
                    winjiRounds = new RoundSelfBasedViewModel
                    {
                        CourseGroupId = (string)_AddedCourse["CourseGroup"]["Id"],
                        EndDate = _Rounds[roundNumber]["EndDate"].ToString(),
                        Location = _Rounds[roundNumber]["Location"].ToString(),
                        MaxCapacity = 0,
                        StartDate = _Rounds[roundNumber]["StartDate"].ToString(),
                        Title = _Rounds[roundNumber]["Name"].ToString(),
                        CopiedFromId = _AddedCourse["Id"].ToString(),
                        TitleTranslations = titleTranslations
                    };

                }
                else
                {
                    winjiRounds = new RoundViewModel
                    {
                        CourseGroupId = (string)_AddedCourse["CourseGroup"]["Id"],
                        EndDate = _Rounds[roundNumber]["EndDate"].ToString(),
                        InstructorId = adminId,
                        Location = _Rounds[roundNumber]["Location"].ToString(),
                        MaxCapacity = 0,
                        StartDate = _Rounds[roundNumber]["StartDate"].ToString(),
                        Title = _Rounds[roundNumber]["Name"].ToString(),
                        CopiedFromId = _AddedCourse["Id"].ToString(),
                        TitleTranslations = titleTranslations
                    };
                }
                AddRound = AddNewRound(winjiRounds);
                Console.WriteLine(AddRound);
                var httpClientReciever = new HttpClient();
                httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenReceiver);
                
                JObjectForRound = JObject.Parse(AddRound);
                Console.WriteLine(JObjectForRound["Id"].ToString());
                string link = $"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/CourseUnitApi/GetUnits?contextId={JObjectForRound["Id"].ToString()}";
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, link);
                Task<HttpResponseMessage> httpResponseMessage = httpClientReciever.SendAsync(httpRequestMessage);
                string message = httpResponseMessage.Result.Content.ReadAsStringAsync().Result;
                
                JArray Filter = JArray.Parse(message);
                if(Filter.Count>0)
                {
                    for (int unitNumber = 0; unitNumber < Filter.Count; unitNumber++)
                    {
                        _callBackRestApi.CallAPi($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/CourseUnitApi/DeleteUnit?unitId={Filter[unitNumber]["Id"].ToString()}", HttpMethod.Get, _tokenReceiver);
                    }
                }
            }
            else
            {
                AddRound = "";
            }
            string roundId = "";
            if(AddRound == "")
            {
                roundId = _AddedCourse["Id"].ToString();
            }
            else
            {
                roundId = JObjectForRound["Id"].ToString();
            }

            int selectedRound =(int)_Rounds[roundNumber]["Id"];
            _winjiUnit.StartAddingUnits(_Rounds[roundNumber]["Name"].ToString(), _AddedCourse["Id"].ToString(), selectedRound,roundId,_tokenSender,_tokenReceiver);
        }

        public string AddNewRound(RoundSelfBasedViewModel roundViewModel)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenReceiver);
            string JsonSerializeForAddCourseGroup = JsonConvert.SerializeObject(roundViewModel);
            var stringContentUnPinnedCourses = new StringContent(JsonSerializeForAddCourseGroup, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessageForAddingCourseGroup = httpClient.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/CourseApi/AddCourse", stringContentUnPinnedCourses).Result;
            string JsonForAddingCourseGroup = httpResponseMessageForAddingCourseGroup.Content.ReadAsStringAsync().Result;
            return JsonForAddingCourseGroup;
        }
    }
}
