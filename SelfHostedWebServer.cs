﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SubversionNet {
    /// <summary>
    /// Self-hosted Web Server class
    /// https://codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server
    /// </summary>
    public class SelfHostedWebServer {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Action<HttpListenerContext> _responderMethod;
        private readonly Action<string> _logMethod;
        private string[] _pathsToListen;
        public bool LogOnEachRequest { get; set; } = false;

        /// <summary>
        /// Creates an HTTP Listener
        /// </summary>
        /// <param name="pathsToListen">Array of paths to listen to (e.g. 'http://localhost:8081/test1/','http://localhost:8081/test2/')</param>
        /// <param name="responseCallback">A callback method which will be called on each request and which allows you to create a corresponding response</param>
        /// <param name="logCallBack">A callback method for providing logs</param>
        public SelfHostedWebServer(string[] pathsToListen, Action<HttpListenerContext> responseCallback, Action<string> logCallBack = null) {
            _logMethod = logCallBack;
            _pathsToListen = pathsToListen;

            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (pathsToListen == null || pathsToListen.Length == 0)
                throw new ArgumentException("pathsToListen are missing!");

            // A responder method is required
            if (responseCallback == null)
                throw new ArgumentException("method is missing!");

            foreach (string s in pathsToListen) {
                string s1 = s;
                if (!s1.EndsWith("/"))
                    s1 = s1 + "/";
                _listener.Prefixes.Add(s1);
            }

            _responderMethod = responseCallback;
        }

        /// <summary>
        /// Start listening
        /// </summary>
        public void Start() {
            _listener.Start();
            ThreadPool.QueueUserWorkItem((o) => {
                log("SelfHostedWebServer - started. Will listen to " + String.Join(" , ", _pathsToListen));
                try {
                    while (_listener.IsListening) {
                        ThreadPool.QueueUserWorkItem((c) => {
                            var ctx = c as HttpListenerContext;
                            try {
                                if (LogOnEachRequest)
                                    log(String.Format("SelfHostedWebServer - request: {0} {1}", ctx.Request.HttpMethod, ctx.Request.Url));
                                _responderMethod(ctx);
                            }
                            catch (Exception ex) {
                                log("SelfHostedWebServer exception: " + ex);
                            }
                            finally {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch (Exception ex) {
                    log("SelfHostedWebServer exception: " + ex);
                }
            });
        }

        /// <summary>
        /// Stop listening
        /// </summary>
        public void Stop() {
            if (_listener.IsListening) {
                _listener.Stop();
                _listener.Close();
                log("SelfHostedWebServer stopped.");
            }
        }

        /// <summary>
        /// Just a sample response callback which writes some text to the output stream.
        /// You should provide your own response callback method in the constructor.
        /// </summary>
        /// <param name="ctx"></param>
        public static void BuiltInResponseCallback(HttpListenerContext ctx) {
            // Is it request with Body?
            String body = "";
            if (ctx.Request.HasEntityBody) {
                using (System.IO.Stream bodyStream = ctx.Request.InputStream) // here we have data
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(bodyStream, ctx.Request.ContentEncoding)) {
                        body = reader.ReadToEnd();
                    }
                }
            }

            String responseString = string.Format("<HTML><BODY>SelfHostedWebServer BuiltInResponseCallback<br>Time: {0}<br>Request:{1}<br>Body: {2}</BODY></HTML>", DateTime.Now, ctx.Request.Url, body);
            byte[] buf = Encoding.UTF8.GetBytes(responseString);
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            ctx.Response.ContentType = "text/html";
        }

        /// <summary>
        /// Log callback
        /// </summary>
        /// <param name="msg"></param>
        void log(string msg) {
            if (_logMethod != null) {
                _logMethod(msg);
            }
        }
    }
}
