using System;
using System.Diagnostics;
using NetMQ;
using NetMQ.Sockets;

namespace heroyan.netmq.pushpull.server.sink
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("====== SINK ======");

            using (var sink = new DealerSocket())
            {
                sink.Bind("tcp://127.0.0.1:7777");

                var startOfBatchTrigger = sink.ReceiveFrameString();
                Console.WriteLine("Seen start of batch");

                var watch = new Stopwatch();
                watch.Start();

                for (int taskNumber = 0; taskNumber < 100; taskNumber++)
                {
                    var workerDoneTrigger = sink.ReceiveFrameString();

                    if (taskNumber % 10 == 0)
                    {
                        Console.Write(":");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }

                watch.Stop();

                Console.WriteLine();
                Console.WriteLine($"Total elapsed time {watch.ElapsedMilliseconds} msec");
                Console.ReadLine();
            }
        }
    }
}
