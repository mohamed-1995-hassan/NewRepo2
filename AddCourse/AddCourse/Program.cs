using AddCourse.CourseUnits;
using AddCourse.Enums;
using AddCourse.Materials;
using AddCourse.Title;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse
{
    class Program
    {
        
        //ConcurrentDictionary<TKey,TValue> Class for source roundid with added roundid
        static Appsetting setting = new Appsetting();
        static string baseUrl = setting.ConfigValue("BaseUrlWinji");
        static string BaseUrlAssessment = setting.ConfigValue("BaseUrlAssessment");
        static List<CourseList> unPinnedCoursesList;
        static List<CourseUnit> courseUnits;
        static int selectedCourseId;
        static JObject o;
        static List<TranslationViewModel> courseTranslationList;
        static string jsonContentAuth;
        #region TokenSender
        static string TokenSender = "ARRAffinity=8f346a422e904eb01749006ab5bdaa3d0e487bca085fb148b154899b8abb52b2; ARRAffinitySameSite=8f346a422e904eb01749006ab5bdaa3d0e487bca085fb148b154899b8abb52b2; .AspNetCore.Antiforgery.w5W7x28NAIs=CfDJ8LNmp9ybYV5ElCuRkSbDN9ZrwR8DTnyy9FFIfsKFvNh7kTjE8VXdxcPtM6ORvvH5vmQUdyQZdRq0u221eTpqZWukwi2t7h9vfbciziTss1JA1vmuWpg19Sf0eYYuWWek8fnAlwKTYaTjcY9I8SbLuB0; _ga=GA1.3.2116816665.1650129194; _gid=GA1.3.1346129852.1650129194; idsrv.session=8A796A687C9EFA6C9DD35D6B859111D8; .AspNetCore.Identity.Application=CfDJ8LNmp9ybYV5ElCuRkSbDN9YlAdIIprge_g7xxPk9WoIO4MK6x4N1i0lK0DGmG9WydOu2tFmyZq3DbHoB0mYdw5cgzHjIHQgR7kJsRNkkO5xQcZJkglsnmvr7l9CPGiQKWEKtkt9GYWN5HP5AQ759ByUrmLF1hEEdUBhIjGG5voP1W4HOdRnZaIWOXNb-qD8vU9N6cTaKtRQz4k3XJ9ViyZpmkIWLJ85VBeT39qW0xCTiR2GLSXTSO5_aRqZgXKPvNlo0ywXsMkgnEiIwlF2AUGU9YrFQUQUdp5pdYM5aGXP6lwBnJKy1puhoG_oX9q9LW8KyYNHfe414_1Mv-kN4MGYPbbsu-Af8y15t33-h_VzvBDCZ87kjR3VTc-7OeCjXg10Expe5XJNaMRQ7ElqzfAh0yX7Fij65tO6IwaihxU-izaFkYlAiWbv4aa8rbR41GaDmfNSwBbwEr3CtLQVaVRD5CsatwF9ADDm0UQ4QkBAGUCkOBjrL4NEtx8NHKcbuQoY476tQMyARYuClGEQHhJVmSPQPkJlcadRzLxO73OFrOsJY66NcwicUxDsB6dg8sUEFwL2bhCBDYwKfRPWQCDKuvkB_2Ss5fxhEQZvyvE_GBaDBwJnTjISWOVww-mYZ_2EMtSl7sv3fLelIsyuzpKTQCIPuRNVuym65XkuNF9x-GpfK5ZYdsvaRWju7SDezqSLJv-vlu5ma67DKRCSGmX-SOOigIsOPc01i-jRs7Vclmqx5q6QXhasSalc_4rQsbou8civw1KVMK4XVvrlMUyCK-bEpCT6aeESVyDJURJ8sz1lxE30bsoAOpK8PnEzW9lz_EiETgFxIliiQyHHDev3CSsrxfCdz_MEdMe546PM_Leqb6kW3gAgWosHTrluk_Pc1OTqWtoT-cpr4HqR7jn1Kdh84Pzqbuiaerc-sP9TczHmsO9rmaT2jjtGVygTELuIu88qxQDKTV7Fqb5XAq_stXV71jdAeYVidj5Sbq4lPcxzxey9Zy2TNGgpHwQOaBa8eeW1hFfo0CzyyLilRwQpHbu8-sHBd_Fc5o-5dw9H_yC9HzAXt2m0oGm2Jx9LFuQ; CurentActiveUserId=33156; ShowAwardingSystemPopup=True; .AspNetCore.Mvc.CookieTempDataProvider=CfDJ8LNmp9ybYV5ElCuRkSbDN9bNzGxxo0v3MSnwY60vPzBQbwQEzXA1mr4elIKxxZGzW4o0pp0Sq-4hRF1VEabLNeGYA81bPHhG0FTdF_CDRim8RP3iiQVFZe3hXHTX53l41Va3nYrDMPiPUaoDbvsW9TlNnHXvmHfIheUis4tYa37O; TrackingToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2NTAxNTA4MjMsImlzcyI6Imh0dHBzOi8veHdpbmppLXRyYWNraW5nLmF6dXJld2Vic2l0ZXMubmV0IiwiYXVkIjoiaHR0cHM6Ly94d2luamkuYXp1cmV3ZWJzaXRlcy5uZXQifQ.XlOlK4AwNTy21NntseaOVNrPRuzxaLTHnjCxLMyVakc; Tracking_Configurations={\"sendMaxLength\":8,\"itemExpiryDuration\":10,\"sendIntervalInMins\":3,\"resendTrialsLimit\":3,\"checkIntervalInMin\":1,\"cookieExpirationInDays\":2}; xsrf-token=CfDJ8LNmp9ybYV5ElCuRkSbDN9btBH9KcgQJO5XTaOPcOiCp9IkzZXOrGv1jUWpAB2ZSSzjifCyzaGEaPUv_REBUyuW6DUxIQuCXKpOLkvZKoPiDmwcE2MJREglNpdJ_ndgywTvYJRBSPumS234TcbaRfDO0pF6x8PQs0W9nVKQ70M88sKvIg-CpGKn7-A4Wd7-u0A";
        #endregion
        #region TokenReciever
        static string TokenReciever = "_ga=GA1.3.1793553984.1648803934; __stripe_mid=e9b54a16-65a7-46db-93e0-b93d49d6f92b08813d; _gid=GA1.3.432102712.1650042619; TrackingToken=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2NTAxMzI5MjYsImlzcyI6Imh0dHBzOi8veHdpbmppLXRyYWNraW5nLmF6dXJld2Vic2l0ZXMubmV0IiwiYXVkIjoiaHR0cHM6Ly94d2luamkuYXp1cmV3ZWJzaXRlcy5uZXQifQ.jD8k6rv1YRFZXf4GS6FUaAsVLtm119Vh0BfZOgr4xtk; Tracking_Configurations={\"sendMaxLength\":8,\"itemExpiryDuration\":10,\"sendIntervalInMins\":3,\"resendTrialsLimit\":3,\"checkIntervalInMin\":1,\"cookieExpirationInDays\":2}; ARRAffinity=8f346a422e904eb01749006ab5bdaa3d0e487bca085fb148b154899b8abb52b2; ARRAffinitySameSite=8f346a422e904eb01749006ab5bdaa3d0e487bca085fb148b154899b8abb52b2; .AspNetCore.Antiforgery.w5W7x28NAIs=CfDJ8LNmp9ybYV5ElCuRkSbDN9bnfpn_PxANH4FTeJtDe83rsds3EwgvRrnw4XQlZ-Gfr_eaZIZlw3wOiepxiYJ1uAlOHKx1h3TllUsvOpJS5lvtfRm2wMdi7SO_cfjmpGtKWBB6yKAFRDWxGwBYiOf2BQA; idsrv.session=FFDC3152E7F3C8CCCD6725F82E9D9E1C; .AspNetCore.Identity.Application=CfDJ8LNmp9ybYV5ElCuRkSbDN9YlRCoL4N4o4DszT9RgTqy2JNyWEcKkqcnM3j-lAcEeRv_wTXs-3O2gs0rb3ElYhDrnoYYyjNfpMzcZmSOZZXTtsYeA3NfJyimOS2fPCi8nhl422GwNuMsVWvtBW3OlVSM8-euH4aG30wAVUatq-kRHf2CxlUdjUojww932t_vubyt_rWUCJciPfYr4G8sNNJjBfSVYEn6iX_fZO9pQn6nbEKKXk4igYXq8yxV8YZU9LzTNu-VR4aL7qw0B6nQ8MB7IjW4enozMhiHt0EFohe03O4l9eSlulOvPksqv_MC6Mva3xnfp0vambLaGkY_T1rLQq-OE5VBIRjMlaDYOZwjeOSGpHoH6JZORtG3B-xVcMQgGafcO439LrtclghN2EAKx-RYx5kIa-9_6Yi_L9a_-GtY0Zc9NV_i9wZdLh88zaFTnfSvR1Psqq97V66YU6u00XXlrlZQwBRjV25V4PsIUG4zfwUvwJUGyaoaXZzdfYfvnxFpF32cJoE9cVKaZR0TXLS8clYPQf6IzO-lwZgg-HO4XKm9aAo2kUWKm8sqPnJmiXV-Qyj6l9HJVNQjSfnRkmP-lqTTaGAYdmmhAo11qrM6Thm9PjUAqR5SaY3LGqPa90hVHtMI9TJi1uqLnzZCh8JgzbJ11peiB7O3y5kwXL7oxdlMmK0zT6VoRtpFkMz4djNMLivt4RwthOnjATcNeoOkzutl2RRfvsuepnsvcEmidEc03_HVDNioa1ITpiARaahuX285qG4pyCXt7olSgRib1TKhEbYmX1tbgzW6Er61gA418dXFH6-qHsMuCb4eCinWl2stvM7SCYjktoDXGBBqjZ_IiToqD7iWsqCKcccBOiovehhiXqi0W-9tkh_T2wkRMKWeYhnMrPdvzeLPSUKJZ_mwctGlMMFiM5ngUaGa97qy7GHfxWgQPLP6300BVIOIqGy_CWkZs4Y8XQet26LBAt6_eca3AW-jYbVa_hjzw9sb9WcSkepfhZAo6KWPB77Hvp7vFLAEf9jKKz3R5416nFtgcOTBEiO7RY4TwL-4QNNt881jV4hTRan0KgCfWR021P0qKlfm-r_1tLCs; CurentActiveUserId=33153; ShowAwardingSystemPopup=True; .AspNetCore.Mvc.CookieTempDataProvider=CfDJ8LNmp9ybYV5ElCuRkSbDN9b0-FNuOzraPH4MgG_sFp4BMFqMXkd_-8tU9jwd63YHlxHoNRfP4ZkEx1zwXoxRhL6-boUpNN7dRxRzwV9Kr6Y9Tz18Fh_wbVgcCodcVvfzOTkweOd-Mz-bvEEs_oNroEmaAUM6ZBrt9axMLzBmFbA6; xsrf-token=CfDJ8LNmp9ybYV5ElCuRkSbDN9YIJwtI0NyH-mQb-SeMfyyfWYtqF-emOT-ZftK2rcrXeBJd52fJh98jYbfjUjgbCqIBd2BHo4-buZhyy2iaBomln4ARGiJC2JIi9t1Fx2BPT8_6aoS3dWKCk89hLmxux3S0LDnKSN_WjffgSZSktA0Jeby68AvVtZIKhZ3_WuiaAA; _gat=1";
        #endregion
        static async Task Main(string[] args)
        {
            

            //add configfile

            //add httpclientfactory
            HttpClient httpClientSender = new HttpClient();
            HttpClient httpClientReciever = new HttpClient();

            //tobe token instead of cookie
            httpClientSender.DefaultRequestHeaders.Add("Cookie", TokenSender);
            string unPinnedCourseJson = setting.ConfigValue("unPinnedCourseJson");
            Console.WriteLine(unPinnedCourseJson);
            var stringContentUnPinnedCourses = new StringContent(unPinnedCourseJson, UnicodeEncoding.UTF8, "application/json");

            //base url to be configurable
            //to use await
            HttpResponseMessage resultForUnpinnedCourses = httpClientSender.PostAsync($"{baseUrl}CourseApi/GetSchoolCoursesList", stringContentUnPinnedCourses).Result;
            if (resultForUnpinnedCourses.IsSuccessStatusCode)
            {
                string stringContentUnPinnedCourse = resultForUnpinnedCourses.Content.ReadAsStringAsync().Result;
                var jsonForUnPinnedCourses = JObject.Parse(stringContentUnPinnedCourse);
                unPinnedCoursesList = JsonConvert.DeserializeObject<List<CourseList>>(jsonForUnPinnedCourses["UnPinnedCourses"].ToString());
            }

            // to be changed with foreachparell
            Parallel.For(0, unPinnedCoursesList.Count, courseNumber =>
            {
                //genaric function to call sendpost/get async
                var httpRequestMessageForCourseDetails = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}CourseApi/GetCourseDetailsAndUpdateLastAccess?courseGroupId=" + unPinnedCoursesList[courseNumber].Id)
                {
                    Headers = { { HeaderNames.Accept, "application/json" }, { HeaderNames.Cookie, TokenSender }, { HeaderNames.AccessControlAllowCredentials, "Access-Control-Allow-Credentials" } }
                };
                var httpResponseForCourseDetails = Task.Run(() => httpClientSender.SendAsync(httpRequestMessageForCourseDetails));
                if (httpResponseForCourseDetails.Result.IsSuccessStatusCode)
                {
                    var contentStreamForCourseDetails = Task.Run(() => httpResponseForCourseDetails.Result.Content.ReadAsStringAsync());
                    o = JObject.Parse(contentStreamForCourseDetails.Result);
                    List<Grade> courseGradesList = JsonConvert.DeserializeObject<List<Grade>>(o["Course"]["CourseGrades"].ToString());
                    courseTranslationList = JsonConvert.DeserializeObject<List<TranslationViewModel>>(o["Course"]["NameTranslations"].ToString());
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

                    Course course = JsonConvert.DeserializeObject<Course>(o["Course"].ToString());
                    course.Status = (int)o["Status"];
                    course.MaxGrade = maxGrade;
                    course.MinGrade = minGrade;
                    course.GradeId = gradeId;
                    course.NameTranslations = courseTranslationList;

                    httpClientReciever.DefaultRequestHeaders.Add("Cookie", TokenReciever);
                    string JsonSerializeForAddCourseGroup = JsonConvert.SerializeObject(course);
                    var stringContentForAddCourseGroup = new StringContent(JsonSerializeForAddCourseGroup, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage httpResponseMessageForAddingCourseGroup = httpClientReciever.PostAsync($"{baseUrl}CourseApi/AddCourseGroup", stringContentForAddCourseGroup).Result;
                    var jsonParsingForAddingCourseGroup = JObject.Parse(httpResponseMessageForAddingCourseGroup.Content.ReadAsStringAsync().Result);
                    if (httpResponseMessageForAddingCourseGroup.IsSuccessStatusCode)
                    {
                        string Rounds = JsonConvert.SerializeObject(o["Courses"]);
                        JArray jArrayRounds = JArray.Parse(Rounds);

                        //for or foreach parrell
                        for (int roundNumber = 0; roundNumber < jArrayRounds.Count; roundNumber++)
                        {
                            string courseRoundJson = JsonConvert.SerializeObject(new
                            {
                                CourseGroupId = (string)jsonParsingForAddingCourseGroup["CourseGroup"]["Id"],
                                EndDate = o["Courses"][roundNumber]["EndDate"].ToString(),
                                InstructorId = (int)o["Course"]["AdminId"],
                                Location = o["Courses"][roundNumber]["Location"].ToString(),
                                MaxCapacity = 12,
                                StartDate = o["Courses"][roundNumber]["StartDate"].ToString(),
                                Title = o["Courses"][roundNumber]["Name"].ToString(),
                                CopiedFromId = jsonParsingForAddingCourseGroup["Id"].ToString(),
                                TitleTranslations = new List<TitleTranslations>{
                                    new TitleTranslations { PropertyValue = o["Courses"][roundNumber]["Name"].ToString() ,
                                    language = new AddCourse.Title.Language{ CultureName = "en-us" , DisplayName = "English" , Name = "English" , UrlPath = "en"}
                                    }
                                }
                            });
                            var courseRound = new StringContent(courseRoundJson, UnicodeEncoding.UTF8, "application/json");
                            HttpResponseMessage addRound = httpClientReciever.PostAsync($"{baseUrl}CourseApi/AddCourse", courseRound).Result;
                            if (addRound.IsSuccessStatusCode)
                            {
                                string contentStreamForRound = addRound.Content.ReadAsStringAsync().Result;
                                JObject jObjectForRound = JObject.Parse(contentStreamForRound);
                                selectedCourseId = (int)o["Courses"][roundNumber]["Id"];
                                var httpRequestMessageForUnits = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}CourseUnitApi/GetUnits?contextId=" + selectedCourseId)
                                {
                                    Headers = { { HeaderNames.Accept, "application/json" }, { HeaderNames.Cookie, TokenSender }, }
                                };

                                Task<HttpResponseMessage> httpResponseMessageForUnits = Task.Run(() => httpClientSender.SendAsync(httpRequestMessageForUnits));
                                var contentStreamForUnits = httpResponseMessageForUnits.Result.Content.ReadAsStringAsync();
                                JArray jArrayForUnits = JArray.Parse(contentStreamForUnits.Result);
                                readCourseUnits(jArrayForUnits);
                                for (int i = 0; i < courseUnits.Count; i++)
                                {
                                    string courseUnitJson = JsonConvert.SerializeObject(new { Title = courseUnits[i].Title, ContextId = jObjectForRound["Id"] });
                                    var stringContentForCourseUnit = new StringContent(courseUnitJson, UnicodeEncoding.UTF8, "application/json");
                                    HttpResponseMessage AddedUnit = httpClientReciever.PostAsync($"{baseUrl}CourseUnitApi/AddUnit", stringContentForCourseUnit).Result;
                                    if (AddedUnit.IsSuccessStatusCode)
                                    {
                                        string resultFromAddedCourseUnit = AddedUnit.Content.ReadAsStringAsync().Result;
                                        JObject jObjectUnit = JObject.Parse(resultFromAddedCourseUnit);
                                        var httpRequestForSelectSession = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}CourseApi/GetSessionsByUnitId?courseId=" + selectedCourseId + "&unitId=" + courseUnits[i].Id)
                                        {
                                            Headers = { { HeaderNames.Accept, "application/json" }, { HeaderNames.Cookie, TokenSender }, }
                                        };
                                        HttpResponseMessage resultSelectedSession = httpClientSender.SendAsync(httpRequestForSelectSession).Result;
                                        string stringValueForSelectedSession = resultSelectedSession.Content.ReadAsStringAsync().Result;
                                        JArray selectedSessionList = JArray.Parse(stringValueForSelectedSession);
                                        for (int j = 0; j < selectedSessionList.Count; j++)
                                        {
                                            var httpRequestForSessionDetails = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}CourseApi/GetSessionDetails?pageNumber=1&pageSize=15&sessionId=" + selectedSessionList[j]["Id"])
                                            {
                                                Headers = { { HeaderNames.Accept, "application/json" }, { HeaderNames.Cookie, TokenSender }, }
                                            };
                                            HttpResponseMessage ResponseWithsessionDetails = httpClientSender.SendAsync(httpRequestForSessionDetails).Result;
                                            string stringValueForSessionDetails = ResponseWithsessionDetails.Content.ReadAsStringAsync().Result;
                                            JObject sessionDetailsObject = JObject.Parse(stringValueForSessionDetails);

                                            string UnitId = (string)jObjectUnit["Id"];
                                            string ContextId = jObjectForRound["Id"].ToString();
                                            string objectiveFileName = (string)sessionDetailsObject["ObjectiveFileName"];
                                            string objectiveFileId = (string)sessionDetailsObject["Session"]["ObjectiveFileId"];
                                            string fileExtention = objectiveFileName.Substring(objectiveFileName.LastIndexOf('.') + 1, (objectiveFileName.Length - (objectiveFileName.LastIndexOf('.') + 1)));
                                            string fileResult = FileManagment(objectiveFileId, objectiveFileName, fileExtention, int.Parse(ContextId).ToString(), "2");
                                            JObject jobjectFileResult = JObject.Parse(fileResult);
                                            //Console.WriteLine(jobjectFileResult);
                                            string lessonBody = JsonConvert.SerializeObject(new
                                            {
                                                ContextId = ContextId,
                                                Duration = (int)selectedSessionList[j]["Duration"],
                                                Title = (string)selectedSessionList[j]["Title"],
                                                //Date = (DateTime)selectedSessionList[j]["Date"],
                                                IsActive = (bool)selectedSessionList[j]["IsActive"],
                                                Type = (int)sessionDetailsObject["Session"]["Type"],
                                                Objectives = sessionDetailsObject["Session"]["Objectives"],
                                                ObjectiveFileId = (string)jobjectFileResult["fileId"],
                                                UnitId = UnitId,
                                            });
                                            var stringContentForLessionBody = new StringContent(lessonBody, UnicodeEncoding.UTF8, "application/json");
                                            HttpResponseMessage ResultFromLessonBody = httpClientReciever.PostAsync($"{baseUrl}CourseApi/AddSession", stringContentForLessionBody).Result;
                                            if (ResultFromLessonBody.IsSuccessStatusCode)
                                            {

                                                string stingValueofLessonResponse = ResultFromLessonBody.Content.ReadAsStringAsync().Result;
                                                JObject jsonValueForTheLesson = JObject.Parse(stingValueofLessonResponse);
                                                string sessionId = jsonValueForTheLesson["Id"].ToString();
                                                JArray materialArray = JArray.Parse(sessionDetailsObject["Materials"].ToString());
                                                //Console.WriteLine(materialArray);

                                                for (int materialcount = 0; materialcount < materialArray.Count; materialcount++)
                                                {
                                                    string materialTitle = materialArray[materialcount]["Title"].ToString();
                                                    string contentParsing = sessionDetailsObject["Materials"][materialcount]["Content"].ToString();
                                                    string material = sessionDetailsObject["Materials"][materialcount].ToString();
                                                    JObject materialJson = JObject.Parse(material);
                                                    JObject parsed = JObject.Parse(contentParsing);

                                                    string materialTypeId = materialArray[materialcount]["MaterialTypeId"].ToString();
                                                    int materialEnum = int.Parse(materialTypeId);
                                                    jsonContentAuth = parsed["activityAuthorizationData"].ToString();

                                                    switch (materialEnum)
                                                    {
                                                        case (int)MaterialEnum.YoutubeMaterial:
                                                            SaveMaterialYoutube(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.AssessmentsMaterial:
                                                        case (int)MaterialEnum.Quiz:
                                                        case (int)MaterialEnum.InCaseActivity:
                                                            jsonContentAuth = parsed["activityAuthorizationData"].ToString();
                                                            JObject Assessment = JObject.Parse(parsed["assessment"].ToString());
                                                            SaveAssessments(Assessment, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId, materialTitle);
                                                            break;
                                                        case (int)MaterialEnum.FileMaterial:
                                                            SaveMaterialFile(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.UrlMaterial:
                                                            SaveMaterialUrl(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.ScormMaterial:
                                                            SaveScormMaterial(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.PoolMaterial:
                                                            SavePoolMaterial(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.VedioMaterial:
                                                            SaveVedioMaterial(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.SurvyMaterial:
                                                            SaveSurvy(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.DiscussionMaterial:
                                                            SaveDiscussion(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                        case (int)MaterialEnum.Html5Material:
                                                            SaveHtml5(materialJson, parsed, httpClientReciever, sessionId, int.Parse(ContextId), materialTypeId);
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }

        private static void SaveAssessments(JObject Assessment,HttpClient httpClientReciever,string contextId,int parentContextId,string materialTypeId,string materialTitle)
        {
            //Console.WriteLine(Assessment);
            JArray Questions = JArray.Parse(Assessment["Questions"]?.ToString());
            Question[] assessmentQuestions = new Question[Questions.Count];
            //Console.WriteLine(Questions);
            NewMaterial newMaterial = new NewMaterial
            {
                content = new Content { BadgeId = null },
                ContextId = contextId,
                ContextTypeId = 1,
                materialTypeId = int.Parse(materialTypeId),
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = materialTitle
            };
            string assessmentMaterialJson = JsonConvert.SerializeObject(newMaterial);
            var stringContentForAssessment = new StringContent(assessmentMaterialJson, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage addedMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForAssessment).Result;
            if (addedMaterial.IsSuccessStatusCode)
            {
                
                JObject AddedMaterialJson = JObject.Parse(addedMaterial.Content.ReadAsStringAsync().Result.ToString());
                JObject jsonContent = JObject.Parse(AddedMaterialJson["Content"].ToString());
                //Console.WriteLine(jsonContent["activityAuthorizationData"]);
                JArray JFileArray = JArray.Parse(Assessment["Files"]?.ToString());
                FileOut[] AssessmentFiles = new FileOut[JFileArray.Count];
                for(int fileCount = 0; fileCount<JFileArray.Count; fileCount++)
                {

                    string fileName = JFileArray[fileCount]["Name"]?.ToString();
                    string fileId = JFileArray[fileCount]["Id"]?.ToString();
                    string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
                    string result = FileManagment(fileId, fileName, extention, "-1", "2");
                    JObject resultJson = JObject.Parse(result);
                    AssessmentFiles[fileCount] = new FileOut
                    {
                        Id = resultJson["Id"]?.ToString(),
                        ContentType = JFileArray[fileCount]["ContentType"]?.ToString(),
                        Size = int.Parse(JFileArray[fileCount]["Size"]?.ToString()),
                        extension = extention,
                        Name = fileName,
                        downloadUrl = $"${BaseUrlAssessment}Files/GetFile/" + fileId + "/" + fileName,
                        playUrl = $"{baseUrl}Files/GetFile/" + fileId + "/" + fileName,
                        isAdded = true,
                        fileNameWithoutExtention = fileName.Split('.')[0]?.ToString(),
                    };
                }
                
                //Copy questions in The assessment

                for(int questionCount=0; questionCount<Questions.Count;questionCount++)
                {

                    File questionfile = null;
                    string downloadUrl = "";
                    string playUrl = "";
                    string fId = "";
                    string fName = "";
                    if (Questions[questionCount]["File"].ToString() != "")
                    {
                        JObject questionFileObject = JObject.Parse(Questions[questionCount]["File"]?.ToString());
                        string size = questionFileObject["Size"]?.ToString();
                        string ContentType = questionFileObject["ContentType"]?.ToString();
                        string id = questionFileObject["Id"]?.ToString();
                        string name = questionFileObject["Name"]?.ToString();
                        string fileExtention = name.Substring(name.LastIndexOf('.') + 1, (name.Length - (name.LastIndexOf('.') + 1)));
                        string questionFile = FileManagment(id, name, fileExtention, "-1", "2");
                        JObject questionFileJson = JObject.Parse(questionFile);
                        downloadUrl = $"{BaseUrlAssessment}Files/GetFile/" + questionFileJson["Id"]?.ToString() + "/" + questionFileJson["Name"]?.ToString();
                        fId = questionFileJson["Id"]?.ToString();
                        fName = questionFileJson["Name"]?.ToString();
                        playUrl = $"{baseUrl}Files/GetFile/" + questionFileJson["Id"]?.ToString() + "/" + questionFileJson["Name"]?.ToString();
                        questionfile = new File
                        {
                            Id = questionFileJson["Id"]?.ToString(),
                            ContentType = ContentType,
                            Name = questionFileJson["Name"]?.ToString(),
                            Size = int.Parse(size)
                        };
                    }

                    //Console.WriteLine(Questions[questionCount]["Type"]?.ToString());
                    string questionType = Questions[questionCount]["Type"]?.ToString();
                    Console.WriteLine(questionType);
                    JArray JOptionArray = new JArray();
                    if (questionType != "Formula" && questionType != "FillInTheBlanks")
                    {
                        JOptionArray = JArray.Parse(Questions[questionCount]["Body"]["Options"].ToString());
                    }

                    object body = null;
                    switch (questionType)
                    {
                        case "MCQ":
                            body = QuestionBodyBasic(JOptionArray);
                            break;
                        case "TrueFalse":
                            body = QuestionBodyBasic(JOptionArray);
                            break;
                        case "MultiResponse":
                            body = QuestionBodyBasic(JOptionArray);
                            break;

                        case "Matching":
                            JArray JFixedOptions = JArray.Parse(Questions[questionCount]["Body"]["FixedOptions"].ToString());
                            body = QuestionBodyMatching(JOptionArray, JFixedOptions);
                            break;
                        case "Order":
                            body = QuestionBodyOrder(JOptionArray);
                            break;
                        case "Image":

                            body = new Body
                            {
                                Options = new Options[] { }
                            };
                            break;
                        case "Formula":
                            JObject jBody = JObject.Parse(Questions[questionCount]["Body"].ToString());
                            JArray jVariables = JArray.Parse(jBody["Variables"].ToString());
                            body = QuestionBodyFormula(jBody, jVariables);
                            break;
                        case "FillInTheBlanks":
                            JObject jBodyBlank = JObject.Parse(Questions[questionCount]["Body"].ToString());
                            body = QuestionBodyFillInTheBlanks(jBodyBlank);
                            break;
                    }

                    if (Questions[questionCount]["File"].ToString() != "")
                    {
                        assessmentQuestions[questionCount] = new QuestionFiles
                        {
                            Title = Questions[questionCount]["Title"]?.ToString(),
                            QuestionTypeUniqueName = Questions[questionCount]["QuestionTypeUniqueName"]?.ToString(),
                            id = 0,
                            DifficultyLevel = Questions[questionCount]["DifficultyLevel"]?.ToString(),
                            DifficultyLevelId = Questions[questionCount]["DifficultyLevelId"]?.ToString(),
                            Grade = int.Parse(Questions[questionCount]["Grade"]?.ToString()),
                            Hint = Questions[questionCount]["Hint"]?.ToString(),
                            IsOccurrenceMessage = Questions[questionCount]["IsOccurrenceMessage"]?.ToString(),
                            ObjectState = Questions[questionCount]["ObjectState"]?.ToString(),
                            Order = int.Parse(Questions[questionCount]["Order"]?.ToString()),
                            Type = Questions[questionCount]["Type"]?.ToString(),
                            clientId = Questions[questionCount]["clientId"]?.ToString(),
                            PreviewMode = Questions[questionCount]["PreviewMode"],
                            indexforcorrect = Questions[questionCount]["indexforcorrect"],
                            CreateMode = Questions[questionCount]["CreateMode"],
                            File = questionfile,
                            DownloadUrl = downloadUrl,
                            PlayUrl = playUrl,
                            FileId = fId,
                            FileName = fName,
                            Body = body,
                            Thumbnail = Questions[questionCount]["Thumbnail"]?.ToString(),
                            //EnableShuffleAnswers = bool.Parse(Questions[questionCount]["EnableShuffleAnswers"]?.ToString())
                        };
                    }
                    else
                    {
                        if (questionType != "Image")
                        {
                            assessmentQuestions[questionCount] = new Question
                            {
                                Title = Questions[questionCount]["Title"]?.ToString(),
                                QuestionTypeUniqueName = Questions[questionCount]["QuestionTypeUniqueName"]?.ToString(),
                                id = 0,
                                DifficultyLevel = Questions[questionCount]["DifficultyLevel"]?.ToString(),
                                DifficultyLevelId = Questions[questionCount]["DifficultyLevelId"]?.ToString(),
                                Grade = int.Parse(Questions[questionCount]["Grade"]?.ToString()),
                                Hint = Questions[questionCount]["Hint"]?.ToString(),
                                IsOccurrenceMessage = Questions[questionCount]["IsOccurrenceMessage"]?.ToString(),
                                ObjectState = Questions[questionCount]["ObjectState"]?.ToString(),
                                Order = int.Parse(Questions[questionCount]["Order"]?.ToString()),
                                Type = Questions[questionCount]["Type"]?.ToString(),
                                clientId = Questions[questionCount]["clientId"]?.ToString(),
                                PreviewMode = Questions[questionCount]["PreviewMode"],
                                indexforcorrect = Questions[questionCount]["indexforcorrect"],
                                CreateMode = Questions[questionCount]["CreateMode"],
                                Body = body,
                                Thumbnail = Questions[questionCount]["Thumbnail"]?.ToString(),
                                //EnableShuffleAnswers = bool.Parse(Questions[questionCount]["EnableShuffleAnswers"]?.ToString())
                            };
                        }
                        else
                        {
                            JObject jQuestionImage = JObject.Parse(Questions[questionCount].ToString());
                            string id = jQuestionImage["QuestionImageFileId"]?.ToString();
                            string name = jQuestionImage["QuestionImageFile"]["Name"]?.ToString();
                            string fileExtention = name.Substring(name.LastIndexOf('.') + 1, (name.Length - (name.LastIndexOf('.') + 1)));
                            string questionImage = FileManagment(id, name, fileExtention, "-1", "2");
                            JObject jquestionImage = JObject.Parse(questionImage);
                            Console.WriteLine(questionImage);
                            string imageContent = jQuestionImage["QuestionImageFile"]["ContentType"]?.ToString();

                            assessmentQuestions[questionCount] = new QuestionImage
                            {
                                Title = Questions[questionCount]["Title"]?.ToString(),
                                QuestionTypeUniqueName = Questions[questionCount]["QuestionTypeUniqueName"]?.ToString(),
                                id = 0,
                                DifficultyLevel = Questions[questionCount]["DifficultyLevel"]?.ToString(),
                                DifficultyLevelId = Questions[questionCount]["DifficultyLevelId"]?.ToString(),
                                Grade = int.Parse(Questions[questionCount]["Grade"]?.ToString()),
                                Hint = Questions[questionCount]["Hint"]?.ToString(),
                                IsOccurrenceMessage = Questions[questionCount]["IsOccurrenceMessage"]?.ToString(),
                                ObjectState = Questions[questionCount]["ObjectState"]?.ToString(),
                                Order = int.Parse(Questions[questionCount]["Order"]?.ToString()),
                                Type = Questions[questionCount]["Type"]?.ToString(),
                                clientId = Questions[questionCount]["clientId"]?.ToString(),
                                PreviewMode = Questions[questionCount]["PreviewMode"],
                                indexforcorrect = Questions[questionCount]["indexforcorrect"],
                                CreateMode = Questions[questionCount]["CreateMode"],
                                Body = body,
                                Thumbnail = Questions[questionCount]["Thumbnail"]?.ToString(),
                                QuestionImageFile = new QuestionImageFile
                                {
                                    ContentType = imageContent,
                                    Id = jquestionImage["Id"]?.ToString(),
                                    Name = jquestionImage["Name"]?.ToString(),
                                    Size = int.Parse(jquestionImage["Size"]?.ToString())
                                }
                            };
                        }
                    }
                }

                Assessment assessment = null;
               if(Assessment["Files"]?.ToString()!="")
                {
                    assessment = new AssessmentFiles
                    {
                        AllowExceedingTime = bool.Parse(Assessment["AllowExceedingTime"]?.ToString()),
                        AllowLateSubmission = bool.Parse(Assessment["AllowLateSubmission"]?.ToString()),
                        AssessmentExternalTool = Assessment["AssessmentExternalTool"],
                        CreatedBy = Assessment["CreatedBy"]?.ToString(),
                        Description = Assessment["Description"]?.ToString(),
                        DueDate = Assessment["DueDate"],
                        Duration = Assessment["Duration"],
                        EnablePublishingScore = bool.Parse(Assessment["EnablePublishingScore"]?.ToString()),
                        GradableItemId = Assessment["GradableItemId"]?.ToString(),
                        Grade = int.Parse(Assessment["Grade"]?.ToString()),
                        GradingMode = Assessment["GradingMode"]?.ToString(),
                        GradingPeriodId = Assessment["GradingPeriodId"]?.ToString(),
                        IsActive = bool.Parse(Assessment["IsActive"]?.ToString()),
                        Mode = int.Parse(Assessment["ModeId"]?.ToString()),
                        IsAssessmentProctored = bool.Parse(Assessment["IsAssessmentProctored"]?.ToString()),
                        IsGroupAssignees = bool.Parse(Assessment["IsGroupAssignees"]?.ToString()),
                        IsProject = bool.Parse(Assessment["IsProject"]?.ToString()),
                        IsSpecificAssignees = bool.Parse(Assessment["IsSpecificAssignees"]?.ToString()),
                        Title = Assessment["Title"]?.ToString(),
                        Type = Assessment["Type"]?.ToString(),
                        Status = Assessment["Status"]?.ToString(),
                        PassingScore = int.Parse(Assessment["PassingScore"]?.ToString()),
                        ProctoredConfiguration = Assessment["ProctoredConfiguration"],
                        Time = Assessment["Time"],
                        RubricId = Assessment["RubricId"],
                        ScormFile = Assessment["ScormFile"],
                        RandomizationCriteriaData = Assessment["RandomizationCriteriaData"],
                        SubmissionType = Assessment["SubmissionType"]?.ToString(),
                        MetaData = new MetaData
                        {
                            CourseId = parentContextId.ToString(),
                            MaterialId = AddedMaterialJson["Id"]?.ToString(),
                            OrganizationId = Assessment["MetaData"]["OrganizationId"]?.ToString(),
                            SchoolId = Assessment["MetaData"]["SchoolId"],
                            SessionId = contextId
                        },
                        Files = AssessmentFiles,
                        Questions = assessmentQuestions
                    };
                }

                else 
                {
                    assessment = new Assessment
                    {
                        AllowExceedingTime = bool.Parse(Assessment["AllowExceedingTime"]?.ToString()),
                        AllowLateSubmission = bool.Parse(Assessment["AllowLateSubmission"]?.ToString()),
                        AssessmentExternalTool = Assessment["AssessmentExternalTool"],
                        CreatedBy = Assessment["CreatedBy"]?.ToString(),
                        Description = Assessment["Description"]?.ToString(),
                        DueDate = Assessment["DueDate"],
                        Duration = Assessment["Duration"],
                        EnablePublishingScore = bool.Parse(Assessment["EnablePublishingScore"]?.ToString()),
                        GradableItemId = Assessment["GradableItemId"]?.ToString(),
                        Grade = int.Parse(Assessment["Grade"]?.ToString()),
                        GradingMode = Assessment["GradingMode"]?.ToString(),
                        GradingPeriodId = Assessment["GradingPeriodId"]?.ToString(),
                        IsActive = bool.Parse(Assessment["IsActive"]?.ToString()),
                        Mode = int.Parse(Assessment["ModeId"]?.ToString()),
                        IsAssessmentProctored = bool.Parse(Assessment["IsAssessmentProctored"]?.ToString()),
                        IsGroupAssignees = bool.Parse(Assessment["IsGroupAssignees"]?.ToString()),
                        IsProject = bool.Parse(Assessment["IsProject"]?.ToString()),
                        IsSpecificAssignees = bool.Parse(Assessment["IsSpecificAssignees"]?.ToString()),
                        Title = Assessment["Title"]?.ToString(),
                        Type = Assessment["Type"]?.ToString(),
                        Status = Assessment["Status"]?.ToString(),
                        PassingScore = int.Parse(Assessment["PassingScore"]?.ToString()),
                        ProctoredConfiguration = Assessment["ProctoredConfiguration"],
                        Time = Assessment["Time"],
                        RubricId = Assessment["RubricId"],
                        ScormFile = Assessment["ScormFile"],
                        RandomizationCriteriaData = Assessment["RandomizationCriteriaData"],
                        SubmissionType = Assessment["SubmissionType"]?.ToString(),
                        MetaData = new MetaData
                        {
                            CourseId = parentContextId.ToString(),
                            MaterialId = AddedMaterialJson["Id"]?.ToString(),
                            OrganizationId = Assessment["MetaData"]["OrganizationId"]?.ToString(),
                            SchoolId = Assessment["MetaData"]["SchoolId"],
                            SessionId = contextId
                        },
                        Questions = assessmentQuestions
                    };
                }
                object[] bulkAssessments = new object[]
                {
                    new
                    {
                        Assessment = assessment,
                        AssessmentRequestID = "8dfafaf1-2e5f-4bc7-9a72-b63888c3d608",
                        EncryptedAuthorizationData = jsonContent["activityAuthorizationData"].ToString(),
                        ObjectState = "added",
                        Permissions = new object[]{ },
                        Questions = new object[]{ }
                    }
                };

                string JsonSerializeForAddingAssesment = JsonConvert.SerializeObject(bulkAssessments);
                JArray TestLog = JArray.Parse(JsonSerializeForAddingAssesment);
                Console.WriteLine(TestLog);
                var stringContentForAddingAssessment = new StringContent(JsonSerializeForAddingAssesment, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessageForAddingAssessment = httpClientReciever.PostAsync($"{BaseUrlAssessment}Assessments", stringContentForAddingAssessment).Result;
                if (httpResponseMessageForAddingAssessment.IsSuccessStatusCode)
                {
                    JArray jsonParsingForAddingAssessment = JArray.Parse(httpResponseMessageForAddingAssessment.Content.ReadAsStringAsync().Result);
                    //Console.WriteLine(jsonParsingForAddingAssessment);
                }
                
            }
        }

        private static void SaveMaterialFile(JObject materialJson,JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId)
        {
            string fileName = parsed["fileName"].ToString();
            string fileId = parsed["fileId"].ToString();
            string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
            string result = FileManagment(fileId, fileName, extention, parentContextId.ToString(), "Course");
            
            JObject jFile = JObject.Parse(result);
            var fileMaterialBody = new
            {
                Content = new
                {
                    fileId = jFile["fileId"]?.ToString(),
                    fileUrl = jFile["fileUrl"]?.ToString()
                },
                ContextId = contextId,
                ContextTypeId = 1,
                Description = materialJson["Description"]?.ToString(),
                IsSpecificAssignees = bool.Parse(materialJson["IsSpecificAssignees"]?.ToString()),
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = materialJson["Title"]?.ToString(),
                fileSize = parsed["fileSize"]?.ToString(),
                materialTypeId = materialTypeId,
            };
            string jsonForfileMaterialBody = JsonConvert.SerializeObject(fileMaterialBody);
            var stringContentForFileMaterial = new StringContent(jsonForfileMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedFileMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForFileMaterial).Result;
            if (AddedFileMaterial.IsSuccessStatusCode)
            {
                //Console.WriteLine(AddedFileMaterial.Content.ReadAsStringAsync().Result);
            }
        }

        private static void SaveMaterialUrl(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId)
        {
            var linkBody = new
            {
                Content = new
                {
                    Url = parsed["FullUrl"].ToString()
                },
                ContextId = contextId,
                ContextTypeId = 1,
                Description = materialJson["Description"].ToString(),
                IsSpecificAssignees = bool.Parse(materialJson["IsSpecificAssignees"].ToString()),
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = materialJson["Title"].ToString(),
                materialTypeId = materialTypeId
            };

            string jsonForLinkMaterialBody = JsonConvert.SerializeObject(linkBody);
            var stringContentForLinkMaterial = new StringContent(jsonForLinkMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedLinkMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForLinkMaterial).Result;
            if (AddedLinkMaterial.IsSuccessStatusCode)
            {
                //Console.WriteLine(AddedLinkMaterial.Content.ReadAsStringAsync().Result);
            }
        }

        private static void SaveVedioMaterial(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId)
        {
            object Content = null;            
            Console.WriteLine(parsed);
            Console.WriteLine("/////////////////////////////////////////////");
            JObject jMaterial = JObject.Parse(materialJson["Content"].ToString());
           
            if(jMaterial["fileId"].ToString() != "")
            {
                string fileName = parsed["fileName"]?.ToString();
                string fileId = parsed["fileId"].ToString();
                string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
                string result = FileManagment(fileId, fileName, extention, parentContextId.ToString(), "Course");
                JObject jFile = JObject.Parse(result);
                Content = new
                {
                    fileId = jFile["fileId"]?.ToString(),
                    fileUrl = jFile["fileUrl"]?.ToString()
                };
            }
            else
            {
                Content = new
                {
                    Subtitle = jMaterial["Subtitle"],
                    Url = jMaterial["Url"],
                    fileId = jMaterial["fileId"]
                };
            }
            var fileMaterialBody = new
            {
                Content = Content,
                ContextId = contextId,
                ContextTypeId = 1,
                Description = materialJson["Description"]?.ToString(),
                IsSpecificAssignees = bool.Parse(materialJson["IsSpecificAssignees"]?.ToString()),
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = materialJson["Title"]?.ToString(),
                fileSize = parsed["fileSize"]?.ToString(),
                materialTypeId = materialTypeId,
            };
            string jsonForVedioMaterialBody = JsonConvert.SerializeObject(fileMaterialBody);
            var stringContentForVedioMaterial = new StringContent(jsonForVedioMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedVedioMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForVedioMaterial).Result;
            if (AddedVedioMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedVedioMaterial.Content.ReadAsStringAsync().Result);
            }

            //Console.WriteLine(result);
        }

        private static void SaveScormMaterial(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId)
        {
            string fileName = parsed["fileName"].ToString();
            string fileId = parsed["fileId"].ToString();
            string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
            string result = FileManagment(fileId, fileName, extention, parentContextId.ToString(), "Course");
            JObject jScormming = JObject.Parse(result);
            JObject jContent = JObject.Parse(materialJson["Content"].ToString());
            Console.WriteLine(jContent["fileSize"]);
            var scormBody = new
            {
                Content = new { fileId = jScormming["fileId"] },
                ContextId = contextId,
                ContextTypeId = 1,
                Description = materialJson["Description"],
                IsSpecificAssignees = materialJson["IsSpecificAssignees"],
                ParentContextId = parentContextId,
                ParentContextTypeId = 2,
                Title = materialJson["Title"],
                fileName = fileName,
                fileSize = jContent["fileSize"],
                materialTypeId = materialTypeId
            };

            string jsonForScormMaterialBody = JsonConvert.SerializeObject(scormBody);
            var stringContentForScromMaterial = new StringContent(jsonForScormMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedScormMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForScromMaterial).Result;
        }

        private static void SaveDiscussion(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId)
        {
            
            var discussionBody = new
            {
                Content = new
                {
                    ContextId = contextId,
                    ContextTypeId = 1,
                    Description = parsed["Description"]?.ToString(),
                    EndDate = parsed["EndDate"],
                    IsSpecificAssignees = bool.Parse(materialJson["IsSpecificAssignees"].ToString()),
                    ParentContextId = parentContextId.ToString(),
                    ParentContextTypeId = 2,
                    StartDate = parsed["EndDate"],
                    Title = parsed["Title"]?.ToString(),
                    type = "Discussion",
                    voiceNoteUpdated = false
                },
                ContextId = contextId,
                ContextTypeId = 1,
                IsSpecificAssignees = bool.Parse(materialJson["IsSpecificAssignees"].ToString()),
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = parsed["Title"]?.ToString(),
                materialTypeId =int.Parse(materialTypeId)
            };
            string jsonForDiscussionMaterialBody = JsonConvert.SerializeObject(discussionBody);
            var stringContentForDiscussionMaterial = new StringContent(jsonForDiscussionMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedDiscussionMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForDiscussionMaterial).Result;
            if (AddedDiscussionMaterial.IsSuccessStatusCode)
            {
            }
        }

        private static void SaveMaterialYoutube(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId)
        {
            JObject content = JObject.Parse(materialJson["Content"].ToString());
            Console.WriteLine(content["Url"]+"    "+ content["ShowThumbnail"]);
            var youTubeBody = new
            {
                AssigneesIds = materialJson["AssigneesIds"],
                Content = new
                {
                    ShowThumbnail = content["ShowThumbnail"],
                    Url = content["Url"]
                },
                ContextId = contextId,
                ContextTypeId = 1,
                Description = materialJson["Description"],
                IsSpecificAssignees = materialJson["IsSpecificAssignees"],
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = materialJson["Title"],
                materialTypeId = materialTypeId,
                type = materialJson["TypeUniqueName"]
            };
            string jsonForYoutubeMaterialBody = JsonConvert.SerializeObject(youTubeBody);
            var stringContentForYoutubeMaterial = new StringContent(jsonForYoutubeMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedMaterialMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForYoutubeMaterial).Result;
            if (AddedMaterialMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedMaterialMaterial.Content.ReadAsStringAsync().Result);
            }
        }
        private static void SavePoolMaterial(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId) 
        {
            JObject jPoll = JObject.Parse(materialJson["Content"].ToString());
            JArray jAPoll = JArray.Parse(jPoll["PollChoices"].ToString());
            var poolChoices = new object[jAPoll.Count];
            for (int poolNumber = 0; poolNumber < jAPoll.Count; poolNumber++)
            {
                poolChoices[poolNumber] = new
                {
                    Body = jAPoll[poolNumber]["Body"],
                    Id = new object(),
                    Index = poolNumber
                };
            }
            var pollBody = new
            {
                Content = new
                {
                    ContextId = contextId,
                    ContextTypeId = 1,
                    Description = jPoll["Description"],
                    EndDate = jPoll["EndDate"],
                    IsSpecificAssignees = materialJson["IsSpecificAssignees"],
                    ParentContextId = parentContextId,
                    ParentContextTypeId = 2,
                    PollChoices = poolChoices,
                    Question = materialJson["Title"]
                },
                ContextId =contextId,
                ContextTypeId = 1,
                IsSpecificAssignees = materialJson["IsSpecificAssignees"],
                ParentContextId =parentContextId,
                ParentContextTypeId = 2,
                Title = materialJson["Title"],
                materialTypeId =materialTypeId
            };
            //Console.WriteLine(pollBody);
            string jsonForPoolMaterialBody = JsonConvert.SerializeObject(pollBody);
            var stringContentForPoolMaterial = new StringContent(jsonForPoolMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedPoolMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForPoolMaterial).Result;
            if (AddedPoolMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedPoolMaterial.Content.ReadAsStringAsync().Result);
            }
        }
        private static void SaveSurvy(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId) 
        {
            //Console.WriteLine(materialJson);
            JObject jContent = JObject.Parse(materialJson["Content"].ToString());
            Console.WriteLine(jContent["Url"]);
            var survyBody = new
            {
                Content = new
                {
                    Url = jContent["Url"]
                },
                ContextId = contextId,
                ContextTypeId = 1,
                Description = materialJson["Description"],
                IsSpecificAssignees = materialJson["IsSpecificAssignees"],
                ParentContextId = parentContextId,
                ParentContextTypeId = 2,
                Title = materialJson["Title"],
                materialTypeId = materialTypeId
            };
            string jsonForSurvyMaterialBody = JsonConvert.SerializeObject(survyBody);
            var stringContentForSurvyMaterial = new StringContent(jsonForSurvyMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedSurvyMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForSurvyMaterial).Result;
            if (AddedSurvyMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedSurvyMaterial.Content.ReadAsStringAsync().Result);
            }
        }

        private static void SaveHtml5(JObject materialJson, JObject parsed, HttpClient httpClientReciever, string contextId, int parentContextId, string materialTypeId)
        {
            string fileName = parsed["fileName"].ToString();
            string fileId = parsed["fileId"].ToString();
            string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
            string result = FileManagment(fileId, fileName, extention, parentContextId.ToString(), "Course");
            Console.WriteLine(result);
            JObject jFile = JObject.Parse(result);
            var fileMaterialBody = new
            {
                Content = new
                {
                    fileId = jFile["fileId"]?.ToString(),
                    fileUrl = jFile["fileUrl"]?.ToString()
                },
                ContextId = contextId,
                ContextTypeId = 1,
                Description = materialJson["Description"]?.ToString(),
                IsSpecificAssignees = bool.Parse(materialJson["IsSpecificAssignees"]?.ToString()),
                ParentContextId = parentContextId.ToString(),
                ParentContextTypeId = 2,
                Title = materialJson["Title"]?.ToString(),
                fileSize = parsed["fileSize"]?.ToString(),
                materialTypeId = materialTypeId,
            };
            string jsonForfileMaterialBody = JsonConvert.SerializeObject(fileMaterialBody);
            var stringContentForFileMaterial = new StringContent(jsonForfileMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedFileMaterial = httpClientReciever.PostAsync($"{baseUrl}MaterialApi/AddNewMaterial", stringContentForFileMaterial).Result;
            if (AddedFileMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedFileMaterial.Content.ReadAsStringAsync().Result);
            }
        }

        public static void readCourseUnits(JArray jArrayForUnits)
        {
            courseUnits = JsonConvert.DeserializeObject<List<CourseUnit>>(jArrayForUnits.ToString());
        }

        public static string FileManagment(string fileId,string fileName,string fileExtention,string hdnModuleId, string hdnModuleContext)
        {
            HttpClient httpClientSender = new HttpClient();
            httpClientSender.DefaultRequestHeaders.Add("Cookie", TokenSender);
            HttpClient httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Add("Cookie", TokenReciever);
            string linkSender = "";
            string linkReciever = "";
            Task<HttpResponseMessage> httpResponseForCourseDetails = null;
            if (hdnModuleId == "-1")
            {
                linkSender = $"{BaseUrlAssessment}Files/GetFile/"+ fileId + "/"+ fileName+ "?clientContextId=" + jsonContentAuth;
                linkReciever = $"{BaseUrlAssessment}Files/UploadFile";
            }
            else
            {
                linkSender = "https://xwinji.azurewebsites.net/Storage/Download/" + fileId;
                linkReciever = "https://xwinji.azurewebsites.net/Storage/UploadDrobzone";
            }
            try
            {
                var httpRequestMessageForCourseDetails = new HttpRequestMessage(HttpMethod.Get, linkSender)
                {
                    Headers = { { HeaderNames.Accept, "application/json" }, { HeaderNames.Cookie, TokenSender }, { HeaderNames.AccessControlAllowCredentials, "Access-Control-Allow-Credentials" } }
                };
                httpResponseForCourseDetails = Task.Run(() => httpClientSender.SendAsync(httpRequestMessageForCourseDetails));
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileStreamContent = new StreamContent(httpResponseForCourseDetails.Result.Content.ReadAsStream());
                fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("file/"+ fileExtention);
                //Add the file
                multipartFormContent.Add(fileStreamContent, name: "file", fileName: fileName);
                //Console.WriteLine(fileStreamContent.ReadAsStream().Length);
                multipartFormContent.Add(new StringContent(hdnModuleContext), name: "hdnModuleContext");
                multipartFormContent.Add(new StringContent("file"), name: "materialType");
                multipartFormContent.Add(new StringContent(hdnModuleId), name: "hdnModuleId");

                //Send it
                HttpResponseMessage response = httpClientReciever.PostAsync(linkReciever, multipartFormContent).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static BodyBase QuestionBodyBasic(JArray JOptionArray)
        {
            Options[] options = new Options[JOptionArray.Count];
            for(int optionCount = 0;optionCount<JOptionArray.Count; optionCount++)
            {
                options[optionCount] = new Options
                {
                    Id = int.Parse(JOptionArray[optionCount]["Id"].ToString()),
                    Title = JOptionArray[optionCount]["Title"].ToString(),
                    IsCorrect = bool.Parse(JOptionArray[optionCount]["IsCorrect"].ToString())
                };
            }
            
            Body body = new Body
            {
                Options = options
            };
            return body;
        }

        public static BodyBase QuestionBodyMatching(JArray JOptionArray, JArray JFixedOptions)
        {
            MatchingOrderOptions[] MatchingOptions = new MatchingOrderOptions[JOptionArray.Count];
            FixedOptions[] FixedOptions = new FixedOptions[JOptionArray.Count];
            for(int optionCount = 0;optionCount<JOptionArray.Count; optionCount++)
            {
                MatchingOptions[optionCount] = new MatchingOrderOptions
                {
                    Id = int.Parse(JOptionArray[optionCount]["Id"].ToString()),
                    Title = JOptionArray[optionCount]["Title"].ToString(),
                    IsCorrect = bool.Parse(JOptionArray[optionCount]["IsCorrect"].ToString()),
                    Order = int.Parse(JOptionArray[optionCount]["Order"].ToString())
                };
                FixedOptions[optionCount] = new FixedOptions
                {
                    Title = JFixedOptions[optionCount]["Title"]?.ToString()
                };
            }
            
            MatchingBody body = new MatchingBody
            {
                 Options = MatchingOptions,
                 FixedOptions = FixedOptions
            };
            return body;
        }

        public static BodyBase QuestionBodyOrder(JArray JOptionArray)
        {
            MatchingOrderOptions[] OrderOptions = new MatchingOrderOptions[JOptionArray.Count];
            for(int optionCount = 0;optionCount<JOptionArray.Count;optionCount++) {
                OrderOptions[optionCount] = new MatchingOrderOptions
                {
                    Id = int.Parse(JOptionArray[optionCount]["Id"].ToString()),
                    Title = JOptionArray[optionCount]["Title"].ToString(),
                    IsCorrect = bool.Parse(JOptionArray[optionCount]["IsCorrect"].ToString()),
                    Order = int.Parse(JOptionArray[optionCount]["Order"].ToString())
                };
            }
            
            BodyOrder body = new BodyOrder
            {
                Options = OrderOptions
            };
            return body;
        }
        public static BodyBase QuestionBodyFormula(JObject jBody, JArray jVariables) 
        {
            Variables[] variables = new Variables[jVariables.Count];
            for(int variableNumber = 0; variableNumber< jVariables.Count; variableNumber++)
            {
                variables[variableNumber] = new Variables
                {
                    Dataset = jVariables[variableNumber]["Dataset"]?.ToString(),
                    exampleValue = int.Parse(jVariables[variableNumber]["exampleValue"]?.ToString()),
                    Max = int.Parse(jVariables[variableNumber]["Max"]?.ToString()),
                    Min = int.Parse(jVariables[variableNumber]["Min"]?.ToString()),
                    Name = jVariables[variableNumber]["Name"]?.ToString(),
                    VariableDataType = int.Parse(jVariables[variableNumber]["VariableDataType"]?.ToString())
                };
            }
            
            FormulaBody body = new FormulaBody
            {
                Variables = variables,
                ErrorMargin = jBody["ErrorMargin"]?.ToString(),
                Formula = jBody["Formula"]?.ToString(),
                FormulaDecimalPoint = jBody["FormulaDecimalPoint"]?.ToString()
            };
            return body;
        }

        public static BodyBase QuestionBodyFillInTheBlanks(JObject jBodyBlank) 
        {
            JArray jBlankAnswer = JArray.Parse(jBodyBlank["BlankAnswers"].ToString());
            BlankAnswers[] blankAnswers = new BlankAnswers[jBlankAnswer.Count];
            for(int blankAnswerNumber = 0; blankAnswerNumber<jBlankAnswer.Count;blankAnswerNumber++)
            {
                JArray jOption = JArray.Parse(jBlankAnswer[blankAnswerNumber]["Options"].ToString());
                MatchingOrderOptions[] options = new MatchingOrderOptions[jOption.Count];
                for(int optionNumber = 0; optionNumber< jOption.Count; optionNumber++)
                {
                    options[optionNumber] = new MatchingOrderOptions
                    {
                        Id = int.Parse(jOption[optionNumber]["Id"]?.ToString()),
                        IsCorrect = bool.Parse(jOption[optionNumber]["IsCorrect"]?.ToString()),
                        Order = int.Parse(jOption[optionNumber]["Order"]?.ToString()),
                        Title = jOption[optionNumber]["Title"]?.ToString()
                    };
                }

                blankAnswers[blankAnswerNumber] = new BlankAnswers
                {
                    Options = options,
                    Order = int.Parse(jBlankAnswer[blankAnswerNumber]["Order"]?.ToString()),
                    tempOption = jBlankAnswer[blankAnswerNumber]["Order"]?.ToString()
                };
            }
            
            
            BlankBody body = new BlankBody
            {
                BlankAnswers = blankAnswers
            };
            return body;
        }
    }    
}
