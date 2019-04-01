using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3starter
{
    public class Social
    {
        public string title { set; get; }
        public string tweet { set; get; }
        public string by { set; get; }
        public string twitter { set; get; }
        public string facebook { set; get; }
    }

    public class QuickLink
    {
        public string title { set; get; }
        public string href { set; get; }
    }

    public class CopyRight
    {
        public string title { set; get; }
        public string html { set; get; }
    }

    public class Footer
    {
        public Social social { set; get; }
        public List<QuickLink> quickLinks { set; get; }
        public CopyRight copyRight { set; get; }
        public string news { set; get; }
    }
}
