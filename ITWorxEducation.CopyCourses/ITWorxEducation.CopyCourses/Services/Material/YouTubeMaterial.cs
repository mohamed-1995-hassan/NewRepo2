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
    public class YouTubeMaterial :IYouTubeMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveMaterialYoutube(JObject materialJson, JObject parsed, string contextId, int parentContextId, string materialTypeId,string tokenReceiver)
        {
            JObject content = JObject.Parse(materialJson["Content"].ToString());
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
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForYoutubeMaterialBody = JsonConvert.SerializeObject(youTubeBody);
            var stringContentForYoutubeMaterial = new StringContent(jsonForYoutubeMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedMaterialMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForYoutubeMaterial).Result;
            if (AddedMaterialMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedMaterialMaterial.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
