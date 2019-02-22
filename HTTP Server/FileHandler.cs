using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Server
{
    class FileHandler
    {
        public string MINE = "text/html";

        private FileInfo f;
        public FileHandler(FileInfo file)
        {
            f = file;
        }
        public byte[] Parse()
        {
            FileStream fs = f.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return d;
        }
    }
}
