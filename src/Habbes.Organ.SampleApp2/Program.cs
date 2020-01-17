using System;
using System.Threading;
using System.Threading.Tasks;
using Habbes.Organ.Host;

namespace Habbes.Organ.SampleApp2
{
    class Program
    {
        static object terminateLock = new object();
        static bool shouldTerminate = false;
        
        static void Main(string[] args)
        {
            var task = Task.Run(RunSample);
            Console.WriteLine("Press any key to terminate");
            Console.ReadKey();
            lock (terminateLock)
            {
                shouldTerminate = true;
            }

            task.Wait();
        }

        static async Task RunSample()
        {
            var peer = new Peer("localhost", 50052);
            peer.StartServer();
            await peer.ConnectDirectory("localhost", 50051);

            var numbers = await peer.GetChannel("numbers");
            long lastIndex = 0;
            while (true)
            {
                lock (terminateLock)
                {
                    if (shouldTerminate)
                    {
                        Console.WriteLine("Shutting down...");
                        peer.StopServer().Wait();
                        break;
                    }
                }

                var result = await numbers.Get(lastIndex, lastIndex + 2);
                foreach (var message in result)
                {
                    var content = message.Content;
                    Array.Reverse(content);
                    var number = BitConverter.ToInt64(content);
                    Console.WriteLine($"App2 read {number} from numbers channel");
                }
                Thread.Sleep(5000);
            }

        }
    }
}