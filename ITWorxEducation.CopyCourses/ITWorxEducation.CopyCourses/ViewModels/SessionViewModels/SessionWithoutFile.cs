using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.SessionViewModels
{
    public class SessionWithoutFile
    {
        public string ContextId { get; set; }
        public object Duration { get; set; }
        public string Title { get; set; }
        //Date = (DateTime)selectedSessionList[j]["Date"],
        public bool IsActive { get; set; } = false;
        public int Type { get; set; }
        public object Objectives { get; set; }
        public string UnitId { get; set; }
    }
}
