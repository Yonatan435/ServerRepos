using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    class Initializer
    {
        public void Run()
        {
            ServiceHost serviceHost = new ServiceHost(typeof(Server));
            serviceHost.Open();
            Console.WriteLine("Server Started.");
        }
    }
}
