using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Broadcast {
    public class Sender {
        private readonly int _port = 0;
        private readonly UdpClient _client = null;
        private readonly IPEndPoint _endpoint = null;
        private readonly int _delay = 0;
        private bool running = false;

        public Sender(int port = 8255, int delay = 2000) {
            _port = port;
            _client = new UdpClient();
            _endpoint = new IPEndPoint(IPAddress.Broadcast, port);
            _delay = delay;
        }

        public void Start() {
            running = true;

            new Thread(() => {
                string ipAddress = GetLocalIPAddress();

                Console.WriteLine("Started sender");

                while (running) {
                    Send(ipAddress);
                    Thread.Sleep(_delay);
                }
            }).Start();
        }

        public void Stop() {
            running = false;
            _client.Close();

            Console.WriteLine("Stopped sender");
        }

        private void Send(string message) {
            byte[] payload = Encoding.UTF8.GetBytes(message);
            _client.Send(payload, payload.Length, _endpoint);
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] Broadcast message (to {_endpoint.Address.ToString()}): {message}");
        }

        private string GetLocalIPAddress() {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
                return null;
            }
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }
}
