using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices.IMaterial
{
    public interface IMaterialManagment
    {
        public void ManageAllMaterial(JObject sessionInfo, string sessionId, string roundId, string tokenReceiver, string tokenSender);
    }
}
