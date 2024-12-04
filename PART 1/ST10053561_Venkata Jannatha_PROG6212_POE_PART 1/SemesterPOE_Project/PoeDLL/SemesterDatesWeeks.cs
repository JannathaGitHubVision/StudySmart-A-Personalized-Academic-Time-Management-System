using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeDLL
{
    public class SemesterDatesWeeks
    {
        private int Weeks;
        private string Date;

        public SemesterDatesWeeks(int weeks, string date)
        {
            Weeks = weeks;
            Date = date;
        }

        public int Weeks1 { get => Weeks; set => Weeks = value; }
        public string Date1 { get => Date; set => Date = value; }
    }
}
