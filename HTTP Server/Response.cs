using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HTTP_Server
{
    class Response
    {
        byte[] data = null;
        private string status; //status
        private string mine; //Mine-type

        private Response(string status, string mine, byte[] data)
        {
            this.status = status;
            this.data = data;
            this.mine = mine;
        }
        
        public static Response From(Request request)
        {
            if (request == null)
                return MakeNullRequest();
            else if (request.Type == "GET")
                return MakeNormalResponse(request, false);
            else
                MakeMethodNotAllowed();
            return MakeNormalResponse(request, true);
        }
        

        private static Response MakeNullRequest()
        {
            
            string file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + "/" + "400.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();

            return new Response("400 Bad Request", "text/html", d);
        }
        private static Response MakeMethodNotAllowed()
        {
            
            string file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + "/" + "405.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();

            return new Response("405 Method Not Allowed", "text/html", d);
        }
        private static Response MakePageNotFound()
        {
            
            string file = Environment.CurrentDirectory + HTTPServer.MSG_DIR + "/" + "404.html";
            FileInfo fi = new FileInfo(file);
            FileStream fs = fi.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();

            return new Response("404 Page Not Found", "text/html", d);
        }
        
        private static Response MakeNormalResponse(Request request, bool NotFound)
        {
            if (!NotFound)
            {
                string file = Environment.CurrentDirectory + HTTPServer.WEB_DIR + request.URL;
                FileInfo f = new FileInfo(file);
                if (f.Exists && f.Extension.StartsWith("."))
                {
                    return MakeFromFile(f);
                }
                else if (!f.Exists && f.Extension.StartsWith("."))
                {
                    MakePageNotFound();
                    Console.WriteLine("Nope File not found");
                }
                else
                {
                    DirectoryInfo di = new DirectoryInfo(f + "/");
                    FileInfo[] files = di.GetFiles();
                    foreach (FileInfo ff in files)
                    {
                        string n = ff.Name;
                        if (n.Contains("default.html") || n.Contains("default.htm") || n.Contains("index.html") || n.Contains("index.htm"))
                        {
                            return MakeFromFile(ff);
                        }
                    }
                }
            }
            else
            {
                return MakePageNotFound(); //if there is no file
            }
            //return null;
            return MakePageNotFound(); //if there is no file
        }
        

        private static Response MakeFromFile(FileInfo f)
        {
            FileStream fs = f.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return new Response("200 OK", "text/html", d);
        }

        public void Post(NetworkStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(String.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n",
                HTTPServer.VERSION, status, HTTPServer.NAME, mine, data.Length));
            writer.Flush(); //We had send the header now send the data
            stream.Write(data, 0, data.Length);
        }
    }
}
