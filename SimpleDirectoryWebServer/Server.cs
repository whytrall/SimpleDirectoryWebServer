using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleDirectoryWebServer {
    public class Server: IDisposable {
        private readonly TcpListener _listener;
        private readonly int _port;

        public Server(IPAddress address, int port) {
            _port = port;
            _listener = new TcpListener(address, port);
        }

        public void Listen() {
            Console.WriteLine($"Listener is starting on port {_port}");
            
            try {
                _listener.Start();
            }
            catch (SocketException e) {
                Console.WriteLine($"An exception happened while starting to listen. " +
                                  $"Error: {e.SocketErrorCode}, {e.Message}");
                Environment.Exit(-(int)e.SocketErrorCode);
            }
            
            while (true) {
                ThreadPool.QueueUserWorkItem(CreateClientForThread, _listener.AcceptTcpClient());
            }
        }

        private static void CreateClientForThread(object state) {
            new Client((TcpClient)state).Process();
        }

        public void Dispose() => _listener?.Stop();
    }
}