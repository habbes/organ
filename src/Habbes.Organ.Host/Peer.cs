using System;
using System.Threading.Tasks;
using grpc = Grpc.Core;
using Habbes.Organ;

namespace Habbes.Organ.Host
{
    public class Peer
    {
        private string id;
        private readonly ChannelContainer channels = new ChannelContainer();
        private DirectoryService.DirectoryServiceClient directory;
        private string host;
        private int port;
        private grpc.Server server;

        public string Id => id;
        public Peer(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public async Task<IChannel> CreateChannel(string channelId, long currencyWindow = 10)
        {
            var channel = new LocalChannel(channelId, currencyWindow);
            var request = new RegisterChannelRequest()
            {
                ChannelId = channelId,
                PeerdId = id
            };
            await directory.RegisterChannelAsync(request);
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
            var request = new GetChannelRequest() { ChannelId = channelId };
            var response = await directory.GetChannelAsync(request);
            var peerConnection = new grpc.Channel(response.ServerLocation.Uri, response.ServerLocation.Port, grpc.ChannelCredentials.Insecure);
            var remotePeerClient = new PeerService.PeerServiceClient(peerConnection);
            
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
            var channel = new grpc.Channel(directoryHost, directoryPort, grpc.ChannelCredentials.Insecure);
            directory = new DirectoryService.DirectoryServiceClient(channel);
            var request = new RegisterPeerRequest()
            {
                ServerLocation = new ServerLocation()
                {
                    Uri = host,
                    Port = port
                }
            };
            var response = await directory.RegisterPeerAsync(request);
            id = response.PeerId;
        }

    }
}
