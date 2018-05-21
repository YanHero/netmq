using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace heroyan.netmq.pushpull.client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== WORKER ======");

            using (var pull = new PullSocket())
            using (var sink = new DealerSocket())
            {
                pull.Connect("tcp://127.0.0.1:8888");
                sink.Connect("tcp://127.0.0.1:7777");

                while (true)
                {
                    var workload = pull.ReceiveFrameString();
                    Thread.Sleep(int.Parse(workload));

                    Console.WriteLine("Sending to Sink");
                    sink.SendFrame(string.Empty);
                }
            }
        }
    }
}
