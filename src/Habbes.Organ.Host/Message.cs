using System;
namespace Habbes.Organ.Host
{
    public class Message: IMessage
    {
        public long Timestamp { get; set; }
        public byte[] Content { get; set; }
    }
}
