using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.Title
{
    public class TitleTranslations
    {

        public string PropertyValue { get; set; }
        public Language language { get; set; }
    }
   public class Language
    {
        public string CultureName { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
    }
}
