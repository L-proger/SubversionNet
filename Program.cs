using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading;
using SubversionNet.Protocol;

namespace SubversionNet {
    public class Program
    {
        static RequestHandler GetRequestHandler(HttpListenerContext request)
        {
            switch (request.Request.HttpMethod.ToLowerInvariant())
            {
                case "options":
                    return new OptionsRequestHandler(request);
                case "propfind":
                    return new PropfindRequestHandler(request);
                case "report":
                    return new ReportRequestHandler(request);
                default:
                    return null;
            }
        }

        static void log(string value) {
            Console.WriteLine(value);
        }

        static void SvnProxy() {

            var listenUrl = "http://127.0.0.1:5422/";
            var targetUrl = "https://svn.antilatency.com:18080/";
            var proxy = new  HttpRedirectingProxy(listenUrl, targetUrl, log);

            var selfHostedWebServer = new SelfHostedWebServer(new string[] { listenUrl }, proxy.ProcessRequest, log);
            selfHostedWebServer.LogOnEachRequest = true;
            selfHostedWebServer.Start();


            while (true) {
                Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {

            SvnProxy();

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:5422/");
            listener.Start();
            Console.WriteLine("Listening...");

            FsServer server = new FsServer(@"F:\FsServerRoot", "/svn");
            server.CreateRepository("TestRepo");
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                Console.WriteLine("### Request headers:");
                for (int i = 0; i < request.Headers.Count; ++i) {
                    var header = request.Headers[i];
                    Console.WriteLine($"{request.Headers.Keys[i]} : {header}");
                }

                var handler = GetRequestHandler(context);
                if (handler != null)
                {
                    handler.handle(server);
                }
                else
                {
                    Console.WriteLine("~~~ Request body:");
                    Console.WriteLine(request.ReadText());
                    Console.WriteLine();
                    throw new Exception("Handler not found for request " + request.HttpMethod);
                }
                Console.WriteLine("Request done!");
            }
            listener.Stop();
        }
    }
}
