using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3starter
{
    public class Degree
    {
        public string degreeName { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<string> concentrations { set; get; }
    }

    public class Degrees
    {
        public List<Degree> undergraduate { set; get; }
        public List<Degree> graduate { set; get; }
    }
}
