using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.FileMaterialsViewModels
{
    public class FileMaterialViewModel
    {
        public string ContextId { get; set; }
        public int ContextTypeId { get; set; } = 1;
        public string Description { get; set; }
        public bool IsSpecificAssignees { get; set; }
        public string ParentContextId { get; set; }
        public int ParentContextTypeId { get; set; } = 2;
        public string Title { get; set; }
        public string fileSize { get; set; }
        public string materialTypeId { get; set; }
        public Content Content { get; set; }
    }
}
