﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpClient
{
    class Program
    {
        const string NewLine = "\r\n";
        static void Main(string[] args)
        {
            //await ReadData();

            TcpListener tcpListener = new TcpListener(
                IPAddress.Loopback, 80);
            tcpListener.Start();

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                ProcesseClientAsync(client);
            }

        }

        public static async Task ProcesseClientAsync(TcpClient client) 
        {
            using (var stream = client.GetStream())
            {
                byte[] buffer = new byte[1000000];
                var lenght = await stream.ReadAsync(buffer, 0, buffer.Length);

                string requestString =
                    Encoding.UTF8.GetString(buffer, 0, lenght);
                Console.WriteLine(requestString);

                //bool sessionSet = false;
                //if (requestString.Contains("sid="))
                //{
                //    sessionSet = true;
                //}

                string html = $"<h1>Hello from TeoServer {DateTime.Now}</h1>" +
                    $"<form method=post><input name=username /><input name=password />" +
                    $"<input type=submit /></form>";

                string response = "HTTP/1.1 200 OK" + NewLine +
                    "Server: TeoServer 2020" + NewLine +
                    //"Location: https://www.google.com" + NewLine +
                    //"Content-Disposition: attachment; filename=teo.txt" + NewLine +
                    "Content-Type: text/html; charset=utf-8" + NewLine +
                    "X-Server-Version: 1.0" + NewLine +
                    //(!sessionSet ? ("Set-Cookie: sid=blaBLA1; Path=/;" + NewLine) : string.Empty) +
                    //"Set-Cookie: sid=traLaLa454; Path=/; Expires= " + DateTime.UtcNow.AddHours(1).ToString("R") + NewLine +
                    "Set-Cookie: sid=traLaLa454; Path=/; HttpOnly; Max-Age=" + (3 * 60) + NewLine +
                    "Content-Lenght: " + html.Length + NewLine +
                    NewLine +
                    html + NewLine;

                byte[] responseByts = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseByts);

                Console.WriteLine(new string('=', 50));

            }
        }
        public static async Task ReadData()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string url = "https://softuni.bg/trainings/3164/csharp-web-basics-september-2020/internal#lesson-18198";
            HttpClient httpClient = new HttpClient();
            //var html = await httpClient.GetStringAsync(url);
            var response = await httpClient.GetAsync(url);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(string.Join(Environment.NewLine, response.Headers.Select(x => x.Key + ": " + x.Value.First())));

            //Console.WriteLine(html);
        }
    }
}
