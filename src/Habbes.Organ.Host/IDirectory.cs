using System.Threading.Tasks;

namespace Habbes.Organ.Host
{
    public interface IDirectory
    {
        Task<string> RegisterPeer(string host, int port);
        Task<(string Host, int Port)> GetChannelLocation(string channelId);
        Task RegisterChannel(string peerId, string channelId);
    }
}