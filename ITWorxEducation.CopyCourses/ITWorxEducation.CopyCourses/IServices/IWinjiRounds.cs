using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices
{
    public interface IWinjiRounds
    {
        public void StartAddingRounds(JArray Rounds, JObject AddedCourse, string tokenSender, string tokenReceiver, int? adminId);
        public void RoundItrationForAdding(int roundNumber, int? adminId);
    }
}
