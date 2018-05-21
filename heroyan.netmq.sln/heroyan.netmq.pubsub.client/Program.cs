using System;
using System.Globalization;
using System.Text;
using NetMQ;
using NetMQ.Sockets;

namespace heroyan.netmq.pubsub.client
{
    class Program
    {
        /// <summary>
        /// 接收到消息事件
        /// </summary>
        private static event Action<string> OnReceiveMsg;

        static void Main(string[] args)
        {
            var zipcode = new Random().Next(0, 10);
            Console.WriteLine($"接收本地天气预报{zipcode}……");

            OnReceiveMsg += Program_OnReceiveMsg;

            using (var subscriber = new SubscriberSocket())
            {
                subscriber.Connect("tcp://127.0.0.1:8888");

                subscriber.Subscribe(zipcode.ToString(CultureInfo.InvariantCulture)); // 设置过滤字符串
                //subscriber.Subscribe(""); // 订阅所有的发布端内容

                while (true)
                {
                    string recMsg = subscriber.ReceiveFrameString(Encoding.UTF8);
                    Console.WriteLine(".");

                    var msgs = recMsg.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (int.Parse(msgs[0]) == zipcode)
                    {
                        OnReceiveMsg(recMsg);
                    }
                }
            }
        }

        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="msg">消息</param>
        private static void Program_OnReceiveMsg(string msg)
        {
            Console.WriteLine($"天气情况:{msg}");
        }
    }
}
