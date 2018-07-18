using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broadcast {
    public class Receiver {
        private readonly int _port = 0;
        private UdpClient _client = null;
        private IPEndPoint endpoint = null;

        public Receiver(int port = 8255) {
            _port = port;
            _client = new UdpClient(port);
            endpoint = new IPEndPoint(IPAddress.Any, _port);
        }

        public void Start() {
            Console.WriteLine("Started receiver");

            StartListening();
        }

        public void Stop() {
            _client.Close();

            Console.WriteLine("Stopped receiver");
        }

        private void StartListening() {
            _client.BeginReceive(Receive, new object());
        }

        private void Receive(IAsyncResult result) {
            byte[] payload = null;
            try {
                payload = _client.EndReceive(result, ref endpoint);
            } catch (ObjectDisposedException) {
                return;
            }
            string message = Encoding.ASCII.GetString(payload);

            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Received message (from {endpoint.Address.ToString()}): {message}");

            StartListening();
        }
    }
}
