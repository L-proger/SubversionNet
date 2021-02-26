using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using SubversionNet.Protocol;

namespace SubversionNet {
    class Program
    {
        static RequestHandler GetRequestHandler(HttpListenerContext request)
        {
            switch (request.Request.HttpMethod.ToLowerInvariant())
            {
                case "options":
                    return new OptionsRequestHandler(request);
                default:
                    return null;
            }
        }

        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            // установка адресов прослушки
            listener.Prefixes.Add("http://*:8080/");
            listener.Start();

            //https://svn.antilatency.com:18080/svn/ACFA0Flasher_Alt_/trunk

            //http://localhost:8080/RepoName/trunk
            Console.WriteLine("Listening...");
            // Note: The GetContext method blocks while waiting for a request.

            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                Console.WriteLine("Request headers:");
                for (int i = 0; i < request.Headers.Count; ++i) {
                    var header = request.Headers[i];
                    Console.WriteLine($"{request.Headers.Keys[i]} : {header}");
                }

                var handler = GetRequestHandler(context);
                if (handler != null)
                {
                    handler.handle();
                }
                else
                {
                    throw new Exception("Handler not found for request " + request.HttpMethod);
                }
                Console.WriteLine("Request done!");
            }




            listener.Stop();
        }
    }
}
