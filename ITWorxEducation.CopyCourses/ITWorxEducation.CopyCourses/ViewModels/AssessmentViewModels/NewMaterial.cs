using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.ViewModels.AssessmentViewModels
{
    public class NewMaterial
    {
        public string ContextId { get; set; }
        public int ContextTypeId { get; set; }
        public string ParentContextId { get; set; }
        public int materialTypeId { get; set; }
        public int ParentContextTypeId { get; set; }
        public string Title { get; set; }
        public NewMaterialContent content { get; set; }
    }
}
