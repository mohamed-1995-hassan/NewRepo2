using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices.IMaterial
{
    public interface IUrlMaterial
    {
        public void SaveMaterialUrl(JObject materialJson, JObject parsed, string contextId, int parentContextId, string materialTypeId, string tokenReceiver);
    }
}
