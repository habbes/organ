using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Habbes.Organ;

namespace Habbes.Organ.Host
{
    public class RemoteChannel: IChannel
    {
        private readonly PeerService.PeerServiceClient client;

        public RemoteChannel(string id, PeerService.PeerServiceClient client)
        {
            this.client = client;
            Id = id;
        }
        
        public string Id { get; }

        public async Task<IEnumerable<IMessage>> Get(long from, long to)
        {
            var request = new GetRequest()
            {
                Channel = Id,
                From = from,
                To = to,
            };
            var response = await this.client.GetAsync(request);
            return response.Messages.Select(m =>
                new Message()
                {
                    Timestamp = m.Timestamp,
                    Content = m.Message.ToByteArray()
                }
            );
        }

        public async Task Put(IMessage message)
        {
            var request = new PutRequest()
            {
                Channel = Id,
                Message = ByteString.CopyFrom(message.Content)
            };
            await this.client.PutAsync(request);
        }
    }
}
