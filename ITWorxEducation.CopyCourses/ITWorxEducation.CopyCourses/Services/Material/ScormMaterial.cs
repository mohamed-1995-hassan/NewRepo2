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
    public class ScormMaterial : IScormMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveScormMaterial(JObject materialJson, JObject parsed,string contextId, int parentContextId, string materialTypeId, string tokenReceiver, string tokenSender)
        {
            string fileName = parsed["fileName"].ToString();
            string fileId = parsed["fileId"].ToString();
            string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
            string result = FileOperations.FileManagment(fileId, fileName, extention, parentContextId.ToString(), "Course", tokenReceiver,tokenSender);
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
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForScormMaterialBody = JsonConvert.SerializeObject(scormBody);
            var stringContentForScromMaterial = new StringContent(jsonForScormMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedScormMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForScromMaterial).Result;
        }
    }
}
