using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Habbes.Organ.Host
{
    public interface IChannel
    {
        string Id { get; }
        Task Put(IMessage message);
        Task<IEnumerable<IMessage>> Get(long from, long to);
    }
}
