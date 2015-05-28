using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using System.Diagnostics;

namespace JobEngine.Client.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        JobEngineClient client = new JobEngineClient();
                        client.Start();
                        Console.WriteLine("Started....");
                        Console.ReadLine();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    System.Threading.Thread.Sleep(1000);

                }
            });

            Console.ReadLine();
        }
    }
}
