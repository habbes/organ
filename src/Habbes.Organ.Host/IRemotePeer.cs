using System.Collections.Generic;
using System.Threading.Tasks;

namespace Habbes.Organ.Host
{
    public interface IRemotePeer
    {
        Task<IEnumerable<IMessage>> GetFromChannel(string channelId, long from, long to);
        Task PutToChannel(string channelId, IMessage message);
    }
}