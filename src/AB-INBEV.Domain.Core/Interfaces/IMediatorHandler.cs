using AB_INBEV.Domain.Core.Commands;
using AB_INBEV.Domain.Core.Events;

namespace AB_INBEV.Domain.Core.Interfaces
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task RaiseEvent<T>(T @event) where T : Event;
    }
}
