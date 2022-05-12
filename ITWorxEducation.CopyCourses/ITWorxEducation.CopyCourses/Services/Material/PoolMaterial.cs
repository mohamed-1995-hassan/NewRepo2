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
    public class PoolMaterial : IPoolMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SavePoolMaterial(JObject materialJson, JObject parsed, string contextId, int parentContextId, string materialTypeId,string tokenReceiver)
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
                ContextId = contextId,
                ContextTypeId = 1,
                IsSpecificAssignees = materialJson["IsSpecificAssignees"],
                ParentContextId = parentContextId,
                ParentContextTypeId = 2,
                Title = materialJson["Title"],
                materialTypeId = materialTypeId
            };
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForPoolMaterialBody = JsonConvert.SerializeObject(pollBody);
            var stringContentForPoolMaterial = new StringContent(jsonForPoolMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedPoolMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForPoolMaterial).Result;
            if (AddedPoolMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedPoolMaterial.Content.ReadAsStringAsync().Result);
            }
        }

    }
}
