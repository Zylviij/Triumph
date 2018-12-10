using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TriumphServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "127.0.0.1";
            int port = 54321;
            Server server = new Server(address, port);
            TestClient client = new TestClient(address, port);

            client.Send("Test!");

            Console.ReadLine();

            List<EndPoint> clients = server.GetAllClients();
            foreach (EndPoint c in clients)
            {
                Console.WriteLine(c.ToString());
            }

            Console.ReadLine();

        }
    }
}
