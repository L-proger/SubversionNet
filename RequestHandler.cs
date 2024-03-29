﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ICSharpCode.SharpZipLib.GZip;

namespace SubversionNet {
    public abstract class RequestHandler {
        protected HttpListenerContext _context;

        public RequestHandler(HttpListenerContext context) {
            _context = context;
        }


        protected string RequestUrl => _context.Request.RawUrl;


        protected string GetBaseRepoUrl(IServer server, IRepository repo)
        {
            return server.UrlPrefix + "/" + repo.Name;
        }

        protected string GetLocalRepoUrl(IServer server, IRepository repo)
        {
            var baseRepoUrl = GetBaseRepoUrl(server, repo);
            if (!RequestUrl.StartsWith(baseRepoUrl))
            {
                throw new Exception("Unexpected base repo URL");
            }
            var result = RequestUrl.Substring(baseRepoUrl.Length + 1);
            return result;
        }


        public abstract void handle(IServer server);


        protected void WriteBadRequestResponse(string text = "BadRequest") {
            // _context.Response.ContentType = "text/xml; charset=\"utf-8\"";
            _context.Response.ContentEncoding = null;
            _context.Response.StatusCode = 400;
            WriteResponse(Encoding.UTF8.GetBytes(text));
        }

        protected void WriteResponse(byte[] data) {
            var acceptEncoding = _context.Request.Headers["Accept-Encoding"];

            if (acceptEncoding == "gzip") {
                _context.Response.AppendHeader("Vary", "Accept-Encoding");
                _context.Response.AppendHeader("Content-Encoding", "gzip");
                var compressedData = GZipCompress(data);

               // File.WriteAllBytes("F:/wtf.gz", compressedData);


                _context.Response.ContentLength64 = compressedData.Length;
                _context.Response.OutputStream.Write(compressedData, 0, compressedData.Length);
            
                _context.Response.OutputStream.Close();
            } else {
                throw new Exception("Unknown response format");
            }
        }

        protected byte[] GZipCompress(byte[] src) {
          
            using var ms = new MemoryStream();
            using var gzs = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(ms);

            gzs.Write(src, 0, src.Length);
            gzs.Flush();
            gzs.Finish();
            return ms.ToArray();
        }


        public static T ParseXml<T>(string text) where T : class {
            if (string.IsNullOrEmpty(text)) {
                return null;
            }

            TextReader treader = new StringReader(text);

            var readerSettings = new XmlReaderSettings { CloseInput = false };
            using var reader = XmlReader.Create(treader, readerSettings);
            reader.MoveToContent();
            return XmlUtils.Deserialize<T>(reader);
        }
    }
}
