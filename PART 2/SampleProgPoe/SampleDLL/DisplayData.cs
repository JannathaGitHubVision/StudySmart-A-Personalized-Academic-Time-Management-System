using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDLL
{
    public class DisplayData
    {
        public int UserID { get; set; }
        public string ModuleName { get; set; }
        public double TotalHours { get; set; }
        public Dictionary<string, double> WeekHours { get; set; }

    }


}
