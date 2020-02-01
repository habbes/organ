using System;
using System.Threading.Tasks;
using grpc = Grpc.Core;
using Habbes.Organ;

namespace Habbes.Organ.Host
{
    public class Peer
    {
        private readonly ChannelContainer channels = new ChannelContainer();
        private IDirectory directory;
        private string host;
        private int port;
        private grpc.Server server;

        public string Id { get; private set; }
        public Peer(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public async Task<IChannel> CreateChannel(string channelId, long currencyWindow = 10)
        {
            var channel = new LocalChannel(channelId, currencyWindow);
            await directory.RegisterChannel(Id, channelId);
            channels.AddChannel(channel);
            return channel;
        }

        public async Task<IChannel> GetChannel(string channelId)
        {
            IChannel channel = channels.GetChannel(channelId);
            if (channel != null)
            {
                return channel;
            }
            var location = await directory.GetChannelLocation(channelId);
            var remotePeerClient = new RemotePeer(location.Host, location.Port);
            
            channel = new RemoteChannel(channelId, remotePeerClient);
            channels.AddChannel(channel);
            return channel;
        }

        public void StartServer()
        {
            server = new grpc.Server()
            {
                Services = { PeerService.BindService(new GrpcPeerService(channels)) },
                Ports = { new grpc.ServerPort(host, port, grpc.ServerCredentials.Insecure) }
            };
            server.Start();
        }

        public Task StopServer()
        {
            return server.ShutdownAsync();
        }

        public async Task ConnectDirectory(string directoryHost, int directoryPort)
        {
            directory = new Directory(directoryHost, directoryPort);
            Id = await directory.RegisterPeer(host, port);
        }

    }
}
