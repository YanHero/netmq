using System;
using NetMQ;
using NetMQ.Sockets;

namespace heroyan.netmq.pushpull.server.ventilator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== VENTILATOR ======");

            using (var push = new PushSocket())
            using (var sink = new DealerSocket())
            {
                push.Bind("tcp://127.0.0.1:8888");
                sink.Connect("tcp://127.0.0.1:7777");

                Console.WriteLine("Press enter when worker are ready");
                Console.ReadLine();

                Console.WriteLine("Sending start of batch to Sink");
                sink.SendFrame("0");

                Console.WriteLine("Sending tasks to workers");

                var rnd = new Random();
                var totalMs = 0;

                for (int taskNumber = 0; taskNumber < 100; taskNumber++)
                {
                    int workload = rnd.Next(0, 100);
                    totalMs += workload;
                    Console.WriteLine("Workload : {0}", workload);
                    push.SendFrame(workload.ToString());
                }

                Console.WriteLine($"Total expected cost : {totalMs} msec");
                Console.WriteLine("Press Enter to quit");
                Console.ReadLine();
            }
        }
    }
}
