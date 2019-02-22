using System;
using System.IO;
using MimeTypes;

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
            MINE = MimeTypeMap.GetMimeType(f.Extension); //get MINE TYPE
            FileStream fs = f.OpenRead();
            BinaryReader reader = new BinaryReader(fs);
            Byte[] d = new Byte[fs.Length];
            reader.Read(d, 0, d.Length);
            fs.Close();
            return d;
        }
    }
}
