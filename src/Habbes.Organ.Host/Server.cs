using System.Threading.Tasks;
using grpc = Grpc.Core;

namespace Habbes.Organ.Host
{
    public class Server
    {
        private grpc.Server grpcServer;
        public Server(string host, int port, ChannelContainer channels)
        {
            Host = host;
            Port = port;
            grpcServer = new grpc.Server()
            {
                Services = { PeerService.BindService(new GrpcPeerService(channels)) },
                Ports = { new grpc.ServerPort(host, port, grpc.ServerCredentials.Insecure) }
            };
        }
        public string Host { get; private set; }
        public int Port { get; private set; }

        public void Start()
        {
            grpcServer.Start();
        }

        public async Task Stop()
        {
            await grpcServer.ShutdownAsync();
        }
    }
}