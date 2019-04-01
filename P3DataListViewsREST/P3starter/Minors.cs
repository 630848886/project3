using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3starter
{
    public class Minor
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<string> courses { set; get; }
        public string note { set; get; }
    }

    public class Minors
    {
        public List<Minor> UgMinors { set; get; }
    }
}
