using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeDLL
{
    public class displayData
    {
        public string Name { get; set; }
        public int TotalHours { get; set; }
        public Dictionary<string, int> Weeks { get; set; } = new Dictionary<string, int>();
    }

}
