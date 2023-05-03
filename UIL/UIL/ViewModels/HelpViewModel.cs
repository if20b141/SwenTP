using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using DAL;

namespace UIL.ViewModels
{
    public class HelpViewModel : BaseViewModel
    {
        DataHandler handler = new DataHandler();
        public string TestString { get; set; }
        public HelpViewModel()
        {
            /*handler.SetTestFunction();
            List<string> list = new List<string>();
            list = handler.GetTestFunction();
            TestString = list[0];
            */
            /* List<string> list = new List<string>();
             list = handler.GetConnectionString(1);
             string test = "";
             for(int i = 0; i < list.Count; i++)
             {
                 test = test + $"{list[i]} " ;
             }
            */
            MapServer server = new BL.MapServer();
            string test = MapServer.MapRequest().Result;
            TestString = test;
        }
    }
}
