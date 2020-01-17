using System.Collections.Generic;

namespace Habbes.Organ.Host
{
    public class ChannelContainer
    {
        private readonly Dictionary<string, IChannel> channels = new Dictionary<string, IChannel>();

        public IChannel GetChannel(string id)
        {
            IChannel channel;
            channels.TryGetValue(id, out channel);
            return channel;
        }

        public void AddChannel(IChannel channel)
        {
            channels[channel.Id] = channel;
        }
    }
}