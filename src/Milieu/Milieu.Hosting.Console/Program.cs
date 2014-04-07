using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web;
using Nancy.Hosting.Self;

namespace Milieu.Hosting.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:8888/milieu/");

            var bootstrapper = new MilieuBootstrapper();

            var host = new NancyHost(bootstrapper, uri);

            host.Start();
            System.Console.WriteLine("Milieu is running at {0}", uri);
            System.Console.WriteLine("Press <Enter> to exit.");
            System.Console.ReadLine();
            System.Console.WriteLine("Goodbye");
        }
    }
}