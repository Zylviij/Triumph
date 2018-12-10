using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TriumphServer
{
    class TestClient
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private EndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);

        private byte[] buffer = new byte[1024 * 8];

        public TestClient(string address, int port)
        {
            this.socket.Connect(IPAddress.Parse(address), port);

            Listen();
        }

        private void RecieveCallback(IAsyncResult asyncResult)
        {
            byte[] data = (byte[])asyncResult.AsyncState;
            int bytes = this.socket.EndReceiveFrom(asyncResult, ref this.endpoint);
            Listen();

            Console.WriteLine("RECV: {0}: {1}, {2}", this.endpoint.ToString(), bytes, Encoding.ASCII.GetString(data));
        }

        private void Listen()
        {
            this.socket.BeginReceiveFrom(this.buffer, 0, this.buffer.Length, SocketFlags.None, ref this.endpoint, this.RecieveCallback, this.buffer);
        }

        public void Send(string text)
        {
            byte[] data = Encoding.ASCII.GetBytes(text);
            this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, (asyncResult) =>
            {
                int bytes = this.socket.EndSend(asyncResult);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, null);
        }
    }
}

