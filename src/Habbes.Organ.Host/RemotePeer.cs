using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Google.Protobuf;
using grpc = Grpc.Core;

namespace Habbes.Organ.Host
{
    public class RemotePeer: IRemotePeer
    {
        private readonly PeerService.PeerServiceClient client;

        public RemotePeer(string host, int port)
        {
            var peerConnection = new grpc.Channel(host, port, grpc.ChannelCredentials.Insecure);
            client = new PeerService.PeerServiceClient(peerConnection);
        }


        public async Task<IEnumerable<IMessage>> GetFromChannel(string channelId, long @from, long to)
        {
            var request = new GetRequest()
            {
                Channel = channelId,
                From = from,
                To = to,
            };
            var response = await client.GetAsync(request);
            return response.Messages.Select(m =>
                new Message()
                {
                    Timestamp = m.Timestamp,
                    Content = m.Message.ToByteArray()
                }
            );
        }

        public async Task PutToChannel(string channelId, IMessage message)
        {
            var request = new PutRequest()
            {
                Channel = channelId,
                Message = ByteString.CopyFrom(message.Content)
            };
            await client.PutAsync(request);
        }
    }
}