using System;
using System.Threading;

namespace Broadcast {
    public class Program {
        public static void Main(string[] args) {
            Sender sender = new Sender();
            sender.Start();
            Receiver receiver = new Receiver();
            receiver.Start();

            while (true) {
                if (Console.KeyAvailable) {
                    ConsoleKeyInfo info = Console.ReadKey(true);
                    if (info.KeyChar == 'x') {
                        receiver.Stop();
                        sender.Stop();
                        Thread.Sleep(2000);
                        break;
                    }
                }
                Thread.Sleep(200);
            }
        }
    }
}
