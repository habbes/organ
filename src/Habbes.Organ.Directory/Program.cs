using System;
using System.Threading.Tasks;
using Grpc.Core;
using Habbes.Organ;

namespace Habbes.Organ.Directory
{
    class Program
    {
        static void Main(string[] args)
        {
            var tracker = new Tracker();
            RunGrpcServer(tracker, "localhost", 50051);
        }

        static void RunGrpcServer(Tracker tracker, string host, int port)
        {
            var server = new Server()
            {
                Services = { DirectoryService.BindService(new GrpcService(tracker)) },
                Ports = { new ServerPort(host, port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine($"Directory gRPC server listening on port {port}");
            Console.WriteLine("Press any key to shutdown.");
            Console.ReadKey();

            Console.WriteLine("Shutting down...");
            server.ShutdownAsync().Wait();
        }
    }
}
