using ITWorxEducation.CopyCourses.Helpers.JsonConverterManagment;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Helpers
{
    public class CallBackRestApi : ICallBackRestApi
    {
        IConfiguration configuration = Program.LoadConfiguration();
        private readonly IHttpClientFactory _clientFactory;
        public CallBackRestApi(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public string CallAPi(string link, HttpMethod methodtype, string Token = "", object paramaters=null)
        {
            //var client = _clientFactory.CreateClient();
            HttpClient client = _clientFactory.CreateClient();
            if(Token!="")
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",Token);
            }
            Task<HttpResponseMessage> response = null;
            if (methodtype == HttpMethod.Post)
            {
                if (paramaters is FormUrlEncodedContent || paramaters is MultipartContent)
                {
                    response = client.PostAsync(link, (FormUrlEncodedContent)paramaters);
                }
                else
                {
                    StringContent stringContent = JsonManagment.SerializeJson(paramaters);
                    Console.WriteLine(stringContent.ReadAsStringAsync().Result);
                    response = client.PostAsync(link,stringContent);
                }
            }
            else if(methodtype == HttpMethod.Get)
            {
                response = client.GetAsync(link);
            }
            return response.Result.Content.ReadAsStringAsync().Result;
        }
    }
}
