using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddCourse.UnitsSessions
{
    class SessionBody
    {
        public int ContextId { get; set; }
        public int Duration { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public bool IsActive { get; set; }
        public int Type { get; set; }
        public string UnitId { get; set; }
        public string Objectives { get; set; }
        public string ObjectiveFileId { get; set; }

    }
}
