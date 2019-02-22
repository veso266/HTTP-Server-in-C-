using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTP_Server
{
    class HTTPServer
    {
        public const string MSG_DIR = "/root/msg"; //Message Directory
        public const string WEB_DIR = "/root/web"; //Web Directory
        public const string VERSION = "HTTP/1.1"; //Server version
        public const string NAME = "DXSW HTTP Server 1.0"; //Server Name (Apache, Nigix, C#)
        private bool running = false; //is the server running
        private TcpListener listener;

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
        }

        private void Run()
        {
            running = true;
            listener.Start();

            while(running)
            {
                Console.WriteLine("Waiting for a connection...");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected: " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);
                HandleClient(client);
                client.Close();
            }

            running = false;
            listener.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());
            string msg = "";
            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + '\n';
            }
            Debug.WriteLine("Request \n" + msg);

            Request req = Request.GetRequest(msg);
            Response resp = Response.From(req);
            resp.Post(client.GetStream());
        }

    }
}
