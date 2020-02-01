using System.Threading.Tasks;
using grpc = Grpc.Core;

namespace Habbes.Organ.Host
{
    public class Directory: IDirectory
    {
        private readonly DirectoryService.DirectoryServiceClient client;
        
        public Directory(string host, int port)
        {
            var channel = new grpc.Channel(host, port, grpc.ChannelCredentials.Insecure);
            client = new DirectoryService.DirectoryServiceClient(channel);
        }
        
        public async Task<string> RegisterPeer(string host, int port)
        {
            var request = new RegisterPeerRequest()
            {
                ServerLocation = new ServerLocation()
                {
                    Uri = host,
                    Port = port
                }
            };
            var response = await client.RegisterPeerAsync(request);
            return response.PeerId;
        }

        public async Task<(string Host, int Port)> GetChannelLocation(string channelId)
        {
            var request = new GetChannelRequest() { ChannelId = channelId };
            var response = await client.GetChannelAsync(request);
            return (response.ServerLocation.Uri, response.ServerLocation.Port);
        }

        public async Task RegisterChannel(string peerId, string channelId)
        {
            var request = new RegisterChannelRequest()
            {
                ChannelId = channelId,
                PeerdId = peerId
            };
            await client.RegisterChannelAsync(request);
        }
    }
}