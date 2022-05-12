using ITWorxEducation.CopyCourses.IServices.IMaterial;
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

namespace ITWorxEducation.CopyCourses.Services.Material
{
    public class SurvyMaterial : ISurvyMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveSurvy(JObject materialJson, JObject parsed, string contextId, int parentContextId, string materialTypeId,string tokenReceiver)
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
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForSurvyMaterialBody = JsonConvert.SerializeObject(survyBody);
            var stringContentForSurvyMaterial = new StringContent(jsonForSurvyMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedSurvyMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForSurvyMaterial).Result;
            if (AddedSurvyMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedSurvyMaterial.Content.ReadAsStringAsync().Result);
            }
        }

    }
}
