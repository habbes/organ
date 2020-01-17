using System;
using System.Threading;
using System.Threading.Tasks;
using Habbes.Organ.Host;

namespace Habbes.Organ.SampleApp
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
            Console.WriteLine("Running App 1");
            var peer = new Peer("localhost", 50053);
            peer.StartServer();
            await peer.ConnectDirectory("localhost", 50051);

            var numbers = await peer.CreateChannel("numbers");
            long current = 0;
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
                var content = BitConverter.GetBytes(current);
                Array.Reverse(content);
                var message = new Message()
                {
                    Content = content,
                    Timestamp = current
                };
                Console.WriteLine($"App 1 has written {current} to numbers channel");
                await numbers.Put(message);
                current++;
                Thread.Sleep(3000);
            }

        }
    }
}
