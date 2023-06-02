using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class tourlogs
    {
        public int id { get; set; }
        public int tourid { get; set; }
        public string time { get; set; }
        public string distance { get; set; }
        public string rating { get; set; }
        public string comment { get; set; }

    }
}
