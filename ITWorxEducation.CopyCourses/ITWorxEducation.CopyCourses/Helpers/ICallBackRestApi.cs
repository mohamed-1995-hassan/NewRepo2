using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.Helpers
{
    public interface ICallBackRestApi
    {
        public string CallAPi(string link, HttpMethod methodtype, string Token = "",object paramaters=null);
    }
}
