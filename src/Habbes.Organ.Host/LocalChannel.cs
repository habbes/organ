using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Habbes.Organ.Host
{
    public class LocalChannel: IChannel
    {
        private readonly List<IMessage> messages = new List<IMessage>();

        public LocalChannel(string id)
        {
            this.Id = id;
        }
        
        public string Id { get; }

        public Task<IEnumerable<IMessage>> Get(long from, long to)
        {
            var result = messages.Where(m => m.Timestamp >= from && m.Timestamp < to);
            return Task.FromResult(result);
        }

        public Task Put(IMessage message)
        {
            messages.Add(message);
            return Task.CompletedTask;
        }

        public Task Put(long timestamp, byte[] body)
        {
            var message = new Message()
            {
                Timestamp = timestamp,
                Content = body
            };
            return Put(message);
        }
    }
}
