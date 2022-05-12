using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse
{
   public class Language
    {
        private string _cultureName;
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public string CultureName
        {
            get
            {
                return _cultureName;
            }
            set
            {
                _cultureName = value;
                if (String.IsNullOrEmpty(_cultureName))
                    Culture = null;
                else
                    Culture = CultureInfo.GetCultureInfo(_cultureName);
            }
        }
        public string DisplayName { get; set; }
        public CultureInfo Culture
        {
            get;
            private set;
        }
    }
}
