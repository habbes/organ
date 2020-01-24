using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Habbes.Organ.Host
{
    public class LocalChannel: IChannel
    {
        private readonly List<IMessage> messages = new List<IMessage>();
        private long mostRecentTimestamp = long.MinValue;

        public LocalChannel(string id, long currencyWindow = 10)
        {
            Id = id;
            CurrencyWindow = currencyWindow;
        }
        
        public string Id { get; }
        public long CurrencyWindow { get; private set; }

        public Task<IEnumerable<IMessage>> Get(long from, long to)
        {
            var result = messages.Where(m => m.Timestamp >= from && m.Timestamp < to);
            return Task.FromResult(result);
        }

        public Task Put(IMessage message)
        {
            messages.Add(message);
            if (message.Timestamp > mostRecentTimestamp) mostRecentTimestamp = message.Timestamp;
            DeleteOldest();
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

        private void DeleteOldest()
        {
            var lastAccepted = mostRecentTimestamp - CurrencyWindow;
            var removed = messages.RemoveAll(m => m.Timestamp < lastAccepted);
            Console.WriteLine($"Removed {removed} elements");
        }
    }
}
