using System;
using System.Threading.Tasks;
using Grpc.Core;
using Habbes.Organ;

namespace Habbes.Organ.Directory
{
    class GrpcService: DirectoryService.DirectoryServiceBase
    {
        Tracker tracker;

        public GrpcService(Tracker tracker)
        {
            this.tracker = tracker;
        }

        public override Task<RegisterPeerResponse> RegisterPeer(RegisterPeerRequest request, ServerCallContext context)
        {
            var peer = tracker.AddPeer(request.ServerLocation.Uri, request.ServerLocation.Port);
            var response = new RegisterPeerResponse()
            {
                Ok = true,
                PeerId = peer.Id
            };
            return Task.FromResult(response);
        }

        public override Task<RegisterChannelResponse> RegisterChannel(RegisterChannelRequest request, ServerCallContext context)
        {
            var channel = tracker.AddChannel(request.PeerdId, request.ChannelId);
            var response = new RegisterChannelResponse()
            {
                Ok = true
            };
            return Task.FromResult(response);
        }

        public override Task<GetChannelResponse> GetChannel(GetChannelRequest request, ServerCallContext context)
        {
            var channel = tracker.GetChannel(request.ChannelId);
            var response = new GetChannelResponse()
            {
                ServerLocation = new ServerLocation()
                {
                    Uri = channel.Peer.Host,
                    Port = channel.Peer.Port
                }
            };
            return Task.FromResult(response);
        }
    }
}