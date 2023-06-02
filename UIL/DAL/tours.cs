using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DAL
{
    public class tours
    {
        public int id { get; set; }
        public string tourname { get; set; }
        public string description { get; set; }
        public string startpoint { get; set; }
        public string endpoint { get; set; }
        public string type { get; set; }
        public string distance { get; set; }
        public string time { get; set; }
        public string information { get; set; }
        public List<tourlogs> logs { get; set; }


    }
}
