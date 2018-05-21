using System;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace heroyan.netmq.pubsub.server
{
    class Program
    {
        private readonly static ManualResetEvent _terminateEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            var weathers = new string[6] { "晴朗", "多云", "阴天", "霾", "雨", "雪" };
            Console.WriteLine("发布多个地区天气预报：");

            using (var publisher = new PublisherSocket())
            {
                publisher.Bind("tcp://127.0.0.1:8888");

                var rng = new Random();

                while (_terminateEvent.WaitOne(1000) == false)
                {
                    var zipcode = rng.Next(0, 10);
                    var temperature = rng.Next(-50, 50);
                    var weatherId = rng.Next(0, 5);

                    var msg = $"{zipcode} {temperature} {weathers[weatherId]}";
                    publisher.SendFrame(msg);

                    Console.WriteLine(msg);
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("exit……");
            _terminateEvent.Set();
        }
    }
}
