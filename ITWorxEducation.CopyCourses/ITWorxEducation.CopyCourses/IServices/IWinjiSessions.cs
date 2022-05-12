using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices
{
    public interface IWinjiSessions
    {
        public void ManageSessions(string selectedRound, string _roundId, string unitIdAdd, string unitId ,string tokenSender, string tokenReceiver);
        public void ReadSessionDetails(JArray selectedSessionList);
        public void PrepareSessionObject(JObject sessionInfo, JArray selectedSessionList, int sessionNumber);
    }
}
