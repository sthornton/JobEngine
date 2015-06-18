using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            JobEngine.Core.ISchedulerService schedulerService = new HangFireSchedulerService();
            schedulerService.Start(@"Server=localhost\SQLEXPRESS;initial catalog=Hangfire;Integrated Security=SSPI");
            Console.WriteLine("Started");
            Console.ReadLine();
        }
    }
}
