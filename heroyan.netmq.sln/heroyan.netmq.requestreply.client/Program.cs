using System;
using NetMQ;
using NetMQ.Sockets;

namespace heroyan.netmq.requestreply.client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var clientSocket = new RequestSocket())
            {
                var num = new Random().Next(0, 100);
                clientSocket.Connect("tcp://127.0.0.1:8888");

                while (true)
                {
                    Console.WriteLine($"{num},Please enter your message:");

                    var message = Console.ReadLine();
                    clientSocket.SendFrame($"{num}:{message}");

                    var answer = clientSocket.ReceiveFrameString();
                    Console.WriteLine($"Answer from server:{answer}");

                    if (message.Equals("exit"))
                    {
                        break;
                    }
                }
            }
        }
    }
}
