using System;
using System.Threading.Tasks;

namespace Sontx.Utils.Executor.Protocol
{
    public interface IObjectTransmitter : IDisposable
    {
        Task PrepareAsync();

        void Send(object obj);

        Task SendAsync(object obj);

        T Receive<T>();

        Task<T> ReceiveAsync<T>();
    }
}