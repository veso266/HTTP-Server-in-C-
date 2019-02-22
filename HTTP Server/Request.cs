using System;

namespace HTTP_Server
{
    class Request
    {
        public String Type { get; set; }
        public String URL { get; set; }
        public String Host { get; set; }
        public String Referrer { get; set; }

        private Request(String type, String url, String host, String referer)
        {
            Type = type;
            URL = url;
            Host = host;
            Referrer = referer;
        }
        public static Request GetRequest(String request)
        {
            if(String.IsNullOrEmpty(request))
                return null;

            String[] tokens = request.Split(' ', '\n'); //FIXME
            string type = tokens[0];
            string url = tokens[1];
            string host = tokens[4];
            string referer = "";
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == "Referer: ")
                {
                    referer = tokens[i + 1];
                    break;
                }
            }

            return new Request(type, url, host, referer);
        }
    }
}
