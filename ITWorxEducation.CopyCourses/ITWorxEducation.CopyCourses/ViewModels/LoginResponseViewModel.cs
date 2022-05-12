using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels
{
    class LoginResponseViewModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string Bearer { get; set; }
        public string scope { get; set; }
    }
}
