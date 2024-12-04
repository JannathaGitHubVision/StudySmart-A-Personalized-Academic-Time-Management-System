using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDLL
{
    public class ModifiedHoursInfo
    {
        public int UserID { get; set; }
        public string ModuleName { get; set; }
        public int WeekNumber { get; set; }
        public int ModifiedHours { get; set; }
        public int RequriedRemhours { get; set; }
        public Dictionary<string, int> WeekModifiedHours { get; set; }
       
    }
}
