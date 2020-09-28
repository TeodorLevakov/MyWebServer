using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //await ReadData();
            const string NewLine = "\r\n";

            TcpListener tcpListener = new TcpListener(
                IPAddress.Loopback, 80);
            tcpListener.Start();

            while (true)
            {
                var client = tcpListener.AcceptTcpClient();
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[1000000];
                    var lenght = stream.Read(buffer, 0, buffer.Length);

                    string requestString =
                        Encoding.UTF8.GetString(buffer, 0, lenght);
                    Console.WriteLine(requestString);

                    string html = $"<h1>Hello from TeoServer {DateTime.Now}</h1>" +
                        $"<form method=post><input name=username /><input name=password />" +
                        $"<input type=submit /></form>";

                    string response = "HTTP/1.1 200 OK" + NewLine +
                        "Server: TeoServer 2020" + NewLine +
                        //"Location: https://www.google.com" + NewLine +
                        //"Content-Disposition: attachment; filename=teo.txt" + NewLine +
                        "Content-Type: text/html; charset=utf-8" + NewLine +
                        "Content-Lenght: " + html.Length + NewLine +
                        NewLine +
                        html + NewLine;

                    byte[] responseByts = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseByts);

                    Console.WriteLine(new string('=', 50));

                }
            }


        }
    }
}
