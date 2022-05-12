using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices.IMaterial
{
    public interface IAssessmentMaterial
    {
        public void SaveAssessments(JObject Assessment, string contextId, int parentContextId, string materialTypeId, string materialTitle, string _tokenReceiver, string _tokenSender, string jsonContentAuth);
    }
}
