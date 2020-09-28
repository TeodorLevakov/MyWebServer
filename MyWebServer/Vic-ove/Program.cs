using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vic_ove
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();

            List<Task> tasks = new List<Task>();

            for (int i = 1; i <= 50; i++)
            {
                var task = Task.Run(async () =>
                {
                    HttpClient httpClient = new HttpClient();
                    var url = $"https://vicove.com/vic-{i}";

                    var responce = await httpClient.GetAsync(url);
                    var vic = await responce.Content.ReadAsStringAsync();

                    Console.WriteLine(vic.Length);
                });
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            
            Console.WriteLine(sw.Elapsed);
            Console.WriteLine();

        }
    }
}
