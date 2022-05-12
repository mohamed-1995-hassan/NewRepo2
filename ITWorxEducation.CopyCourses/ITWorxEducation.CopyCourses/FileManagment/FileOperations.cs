using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.FileManagment
{
    public class FileOperations
    {
        
        public static string FileManagment(string fileId, string fileName, string fileExtention, string hdnModuleId, string hdnModuleContext, string TokenReceiver, string TokenSender="",string jsonContentAuth="")
        {
            IConfiguration configuration = Program.LoadConfiguration();
            HttpClient httpClientSender = new HttpClient();
            httpClientSender.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenSender);
            HttpClient httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenReceiver);
            string linkSender = "";
            string linkReciever = "";
            Task<HttpResponseMessage> httpResponseForCourseDetails = null;
            if (hdnModuleId == "-1")
            {
                linkSender = $"{configuration["AuthorizationTokenSource:BaseURLAssessment"]}/api/Files/GetFile/" + fileId + "/" + fileName + "?clientContextId=" + jsonContentAuth;
                linkReciever = $"{configuration["AuthorizationTokenDestination:BaseURLAssessment"]}/api/Files/UploadFile";
            }
            else
            {
                linkSender = $"{configuration["AuthorizationTokenSource:BaseURL"]}/Storage/Download/" + fileId;
                linkReciever = $"{configuration["AuthorizationTokenDestination:BaseURL"]}/Storage/UploadDrobzone";
            }
            try
            {
                var httpRequestMessageForCourseDetails = new HttpRequestMessage(HttpMethod.Get, linkSender)
                {
                    Headers = { { HeaderNames.Accept, "application/json" }, { HeaderNames.Cookie, TokenSender }, { HeaderNames.AccessControlAllowCredentials, "Access-Control-Allow-Credentials" } }
                };
                httpResponseForCourseDetails = Task.Run(() => httpClientSender.SendAsync(httpRequestMessageForCourseDetails));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                string fileType = "";
                if(fileExtention == "mp4"|| fileExtention == "Webm"|| fileExtention=="ogg")
                {
                    fileType = "video";
                }
                else
                {
                    fileType = "file";
                }
                var fileStreamContent = new StreamContent(httpResponseForCourseDetails.Result.Content.ReadAsStream());
                fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(fileType+ "/" + fileExtention);
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
    }
}
