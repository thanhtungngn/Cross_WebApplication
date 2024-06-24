using System.Net.Sockets;

namespace Cross_WebApplication.Services
{
    public interface ITcpListenerService
    {
        void Start();
        void Stop();
    }
}
