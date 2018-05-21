using System;
using NetMQ;
using NetMQ.Sockets;

namespace heroyan.netmq.requestreply.server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serverSocket = new ResponseSocket())
            {
                serverSocket.Bind("tcp://127.0.0.1:8888");

                while (true)
                {
                    var rcMsg = serverSocket.ReceiveFrameString();
                    Console.WriteLine($"Receive message :\r\n{rcMsg}\r\n");

                    var msg = rcMsg.Split(':')[1].ToLower();

                    #region 根据接收到的消息，返回不同的信息

                    switch (msg)
                    {
                        case "hello":
                            serverSocket.SendFrame("World");
                            break;
                        case "hi":
                            serverSocket.SendFrame("你好");
                            break;
                        default:
                            serverSocket.SendFrame(msg);
                            break;
                    }

                    #endregion

                    if (msg.Equals("exit"))
                    {
                        break;
                    }
                }
            }
        }
    }
}
