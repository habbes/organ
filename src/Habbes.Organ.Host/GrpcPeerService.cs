using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Habbes.Organ;

namespace Habbes.Organ.Host
{
    internal class GrpcPeerService: PeerService.PeerServiceBase
    {
        private ChannelContainer channels;
        
        public GrpcPeerService(ChannelContainer channels)
        {
            this.channels = channels;
        }

        public override async Task<PutResponse> Put(PutRequest request, ServerCallContext context)
        {
            var channel = channels.GetChannel(request.Channel);
            var message = new Message()
            {
                Timestamp = request.Timestamp,
                Content = request.Message.ToByteArray()
            };
            await channel.Put(message);
            var response = new PutResponse() { ResponseStatus = ResponseHelpers.CreateOkStatus() };
            return response;
        }

        public override async Task<GetResponse> Get(GetRequest request, ServerCallContext context)
        {
            var channel = channels.GetChannel(request.Channel);
            var messages = await channel.Get(request.From, request.To);
            var response = new GetResponse() { ResponseStatus = ResponseHelpers.CreateOkStatus() };
            foreach (var message in messages)
            {
                response.Messages.Add(new ChannelMessage()
                {
                    Timestamp = message.Timestamp,
                    Message = ByteString.CopyFrom(message.Content)
                });
            }

            return response;
        }

    }
}
