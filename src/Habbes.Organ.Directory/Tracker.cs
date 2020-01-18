using System;
using System.Collections.Generic;

namespace Habbes.Organ.Directory
{
    class Tracker
    {
        Dictionary<string, Peer> peers = new Dictionary<string, Peer>();
        Dictionary<string, Channel> channels = new Dictionary<string, Channel>();

        public Peer AddPeer(string host, int port)
        {
            var peer = new Peer()
            {
                Id = Guid.NewGuid().ToString(),
                Host = host,
                Port = port
            };
            peers.Add(peer.Id, peer);
            Console.WriteLine($"Added peer {port}:{host}, id {peer.Id}");
            return peer;
        }

        public Peer GetPeer(string id)
        {
            return peers[id];
        }

        public Channel AddChannel(string peerId, string channelId)
        {
            var peer = peers[peerId];
            var channel = new Channel()
            {
                Id = channelId,
                Peer = peer
            };
            channels.Add(channelId, channel);
            Console.WriteLine($"Added channel {channelId}, {channel.Peer.Id}");
            return channel;
        }

        public Channel GetChannel(string channelId)
        {
            var channel = channels[channelId];
            Console.WriteLine($"Getting channel {channelId} {channel.Peer.Id}");
            return channel;
        }
    }
}