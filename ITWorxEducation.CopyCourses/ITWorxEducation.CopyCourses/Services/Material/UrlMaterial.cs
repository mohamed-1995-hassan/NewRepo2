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
    public class UrlMaterial : IUrlMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveMaterialUrl(JObject materialJson, JObject parsed,string contextId, int parentContextId, string materialTypeId, string tokenReceiver)
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
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForLinkMaterialBody = JsonConvert.SerializeObject(linkBody);
            var stringContentForLinkMaterial = new StringContent(jsonForLinkMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedLinkMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForLinkMaterial).Result;
            if (AddedLinkMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedLinkMaterial.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
