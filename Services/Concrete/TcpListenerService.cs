using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cross_WebApplication.Services
{


    public class TcpListenerService : ITcpListenerService
    {
        private readonly int _port;
        private TcpListener _listener;
        private bool _isRunning;

        public TcpListenerService(int port)
        {
            _port = port;
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);
            _listener.Start();
            _isRunning = true;
            Console.WriteLine($"TCP listener started on port {_port}");

            Task.Run(() => ListenForClients());
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            Console.WriteLine("TCP listener stopped");
        }

        private async Task ListenForClients()
        {
            while (_isRunning)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                await ProcessClient(client);
            }
        }

        private async Task ProcessClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received data: {data}");

                    // Here you can process the received data, e.g., save to MongoDB
                    // HandleEvent(data);

                    // Respond back if needed
                    // byte[] response = Encoding.UTF8.GetBytes("Message received");
                    // await stream.WriteAsync(response, 0, response.Length);
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing client: {ex.Message}");
            }
        }
    }

}