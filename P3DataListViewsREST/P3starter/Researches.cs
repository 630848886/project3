using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3starter
{
    public class ResearchByInterestArea
    {
        public string areaName { get; set; }
        public List<string> citations { get; set; }
    }

    public class ResearchByFaculty
    {
        public string facultyName { get; set; }
        public string username { get; set; }
        public List<string> citations { get; set; }
    }


    public class Researches
    {
        public List<ResearchByInterestArea> byInterestArea { get; set; }
        public List<ResearchByFaculty> byFaculty { get; set; }
    }
}
