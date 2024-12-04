using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeDLL
{
    public class ModulesInfo
    {
        private string ModuleName;
        private string ModuleCode;
        private int ModuleCredits;
        private int ModuleClassHrs;

        public ModulesInfo(string moduleName, string moduleCode, int moduleCredits, int moduleClassHrs)
        {
            ModuleName = moduleName;
            ModuleCode = moduleCode;
            ModuleCredits = moduleCredits;
            ModuleClassHrs = moduleClassHrs;
        }

        public string ModuleName1 { get => ModuleName; set => ModuleName = value; }
        public string ModuleCode1 { get => ModuleCode; set => ModuleCode = value; }
        public int ModuleCredits1 { get => ModuleCredits; set => ModuleCredits = value; }
        public int ModuleClassHrs1 { get => ModuleClassHrs; set => ModuleClassHrs = value; }
    }
 

}
