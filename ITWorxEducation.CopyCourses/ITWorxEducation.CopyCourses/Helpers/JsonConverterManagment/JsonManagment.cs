using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Helpers.JsonConverterManagment
{
    class JsonManagment
    {
        public static StringContent SerializeJson(object json)
        {
            if(json is not string)
            {
                json = JsonConvert.SerializeObject(json);
            }
            StringContent stringContent = new StringContent(json.ToString(), UnicodeEncoding.UTF8, "application/json");
            return stringContent;
        }

        public static TJson DeserializeJson<TJson>(string jsonResult)
        {

            TJson convertedObject = JsonConvert.DeserializeObject<TJson>(jsonResult);
            return convertedObject;
        }
        
    }

}
