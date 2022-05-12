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
    public class DiscussionMaterial : IDiscussionMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveDiscussion(JObject materialJson, JObject parsed,string contextId, int parentContextId, string materialTypeId,string tokenReceiver)
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
                materialTypeId = int.Parse(materialTypeId)
            };
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForDiscussionMaterialBody = JsonConvert.SerializeObject(discussionBody);
            var stringContentForDiscussionMaterial = new StringContent(jsonForDiscussionMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedDiscussionMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForDiscussionMaterial).Result;
            if (AddedDiscussionMaterial.IsSuccessStatusCode)
            {
            }
        }

    }
}
