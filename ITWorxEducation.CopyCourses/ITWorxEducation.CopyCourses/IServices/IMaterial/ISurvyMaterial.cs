using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices.IMaterial
{
    public interface ISurvyMaterial
    {
        public void SaveSurvy(JObject materialJson, JObject parsed, string contextId, int parentContextId, string materialTypeId, string tokenReceiver);

    }
}
