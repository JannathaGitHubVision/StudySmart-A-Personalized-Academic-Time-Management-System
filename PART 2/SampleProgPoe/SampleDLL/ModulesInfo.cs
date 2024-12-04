using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDLL
{
    public class ModulesInfo
    {

        public int UsersID { get; set; }
        public string ModuleName;
        public string ModuleCode;
        public int ModuleCredits;
        public int ModuleClassHrs;
        public int Weeks;
        public string Date;
        public int selfStudyHr { get; set; }

        public string ModuleName1 { get => ModuleName; set => ModuleName = value; }
        public string ModuleCode1 { get => ModuleCode; set => ModuleCode = value; }
        public int ModuleCredits1 { get => ModuleCredits; set => ModuleCredits = value; }
        public int ModuleClassHrs1 { get => ModuleClassHrs; set => ModuleClassHrs = value; }
        public int Weeks1 { get => Weeks; set => Weeks = value; }
        public string Date1 { get => Date; set => Date = value; }
    }
}
