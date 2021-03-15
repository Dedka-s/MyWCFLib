using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MyWCFLibHost
{
    class Program
    {
        static void Main()
        {
            using (var host = new ServiceHost(typeof(MyWCFLib.MyWCFLib)))
            {
                host.Open();

                Console.WriteLine("Host Started");
                Console.ReadLine();
            }
        }
    }
}
