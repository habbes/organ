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
        private IRemotePeer client;

        public RemoteChannel(string id, IRemotePeer client)
        {
            this.client = client;
            Id = id;
        }
        
        public string Id { get; }

        public async Task<IEnumerable<IMessage>> Get(long from, long to)
        {
            var messages = await client.GetFromChannel(Id, from, to);
            return messages;
        }

        public async Task Put(IMessage message)
        {
            await client.PutToChannel(Id, message);
        }
    }
}
