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
                case "propfind":
                    return new PropfindRequestHandler(request);
                default:
                    return null;
            }
        }

        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:8080/");
            listener.Start();
            Console.WriteLine("Listening...");

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
