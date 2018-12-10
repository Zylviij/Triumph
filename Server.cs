using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TriumphServer
{
    class Server
    {
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private EndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);

        private byte[] buffer = new byte[1024 * 8];

        private List<EndPoint> clients = new List<EndPoint>();

        public Server(string address, int port)
        {
            this.socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            this.socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));

            Listen();
        }

        private void RecieveCallback(IAsyncResult asyncResult)
        {
            byte[] data = (byte[]) asyncResult.AsyncState;
            int bytes = this.socket.EndReceiveFrom(asyncResult, ref this.endpoint);
            clients.Add(this.endpoint);
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
                string so = (string) asyncResult.AsyncState;
                int bytes = this.socket.EndSend(asyncResult);
                Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, null);
        }

        public List<EndPoint> GetAllClients()
        {
            return this.clients;
        }
    }   
}
