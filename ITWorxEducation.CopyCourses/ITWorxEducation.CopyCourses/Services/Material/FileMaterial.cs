using ITWorxEducation.CopyCourses.FileManagment;
using ITWorxEducation.CopyCourses.IServices.IMaterial;
using ITWorxEducation.CopyCourses.ViewModels.FileMaterialsViewModels;
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
    public class FileMaterial : IFileMaterial
    {
        IConfiguration configuration = Program.LoadConfiguration();
        public void SaveMaterialFile(JObject materialJson, JObject parsed, string contextId, int parentContextId, string materialTypeId,string tokenReceiver, string tokenSender)
        {
            string fileName = parsed["fileName"].ToString();
            string fileId = parsed["fileId"].ToString();
            string extention = fileName.Substring(fileName.LastIndexOf('.') + 1, (fileName.Length - (fileName.LastIndexOf('.') + 1)));
            string result = FileOperations.FileManagment(fileId, fileName, extention, parentContextId.ToString(), "Course",tokenReceiver,tokenSender);
            JObject jFile = JObject.Parse(result);
            var fileMaterialBody = new FileMaterialViewModel
            {
                Content = new Content
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
            var httpClientReciever = new HttpClient();
            httpClientReciever.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenReceiver);
            string jsonForfileMaterialBody = JsonConvert.SerializeObject(fileMaterialBody);
            var stringContentForFileMaterial = new StringContent(jsonForfileMaterialBody, UnicodeEncoding.UTF8, "application/json");
            HttpResponseMessage AddedFileMaterial = httpClientReciever.PostAsync($"{configuration["AuthorizationTokenDestination:BaseURL"]}/api/MaterialApi/AddNewMaterial", stringContentForFileMaterial).Result;
            if (AddedFileMaterial.IsSuccessStatusCode)
            {
                Console.WriteLine(AddedFileMaterial.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
