using System;
namespace Habbes.Organ.Host
{
    public interface IMessage
    {
        long Timestamp { get; set; }
        byte[] Content { get; set; }
    }
}
