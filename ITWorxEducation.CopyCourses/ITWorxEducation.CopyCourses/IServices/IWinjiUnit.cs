using ITWorxEducation.CopyCourses.ViewModels.UnitsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITWorxEducation.CopyCourses.IServices
{
    public interface IWinjiUnit
    {
        public void StartAddingUnits(string roundName, string courseId, int selectedRound, string roundId, string tokenSender, string tokenReceiver);
        public List<UnitViewModel> ReadAllUnits();
        public void AddUnitCycle(List<UnitViewModel> unitViewModels);
    }
}
