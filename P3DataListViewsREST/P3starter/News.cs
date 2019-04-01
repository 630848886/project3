using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3starter
{
    public class New
    {
        public string date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
    public class News
    {
        public List<New> older { get; set; }
        public List<New> year { get; set; }
    }
}
