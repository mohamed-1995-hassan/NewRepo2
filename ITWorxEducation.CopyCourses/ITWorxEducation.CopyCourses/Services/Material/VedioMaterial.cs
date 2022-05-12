using ITWorxEducation.CopyCourses.FileManagment;
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
    public class VedioMaterial : IVedioMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveVedioMaterial(JObject materialJson, JObject parsed,string contextId, int parentContextId, string materialTypeId,string tokenReceiver,string tokenSender)
        {
            //Console.WriteLine(materialJson);
            object Content = null;
            JObject jMaterial = JObject.Parse(materialJson["Content"].ToString());
            Console.WriteLine(jMaterial);

            if (!string.IsNullOrEmpty(jMaterial["fileId"]?.ToString()))
            {
                string fileName = parsed["fileName"]?.ToString();
                string fileId = parsed["fileId"].ToString();
                string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
                string result = FileOperations.FileManagment(fileId, fileName, extention, parentContextId.ToString(), "Course",tokenReceiver,tokenSender);
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
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForVedioMaterialBody = JsonConvert.SerializeObject(fileMaterialBody);
            var stringContentForVedioMaterial = new StringContent(jsonForVedioMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedVedioMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForVedioMaterial).Result;
            if (AddedVedioMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedVedioMaterial.Content.ReadAsStringAsync().Result);
            }

            //Console.WriteLine(result);
        }
    }
}
