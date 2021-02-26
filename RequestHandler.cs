using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;

namespace SubversionNet {
    abstract class RequestHandler {
        protected HttpListenerContext _context;
        public RequestHandler(HttpListenerContext context) {
            _context = context;
        }

        public abstract void handle();

        protected void WriteResponse(byte[] data) {
            var acceptEncoding = _context.Request.Headers["Accept-Encoding"];

            if (acceptEncoding == "gzip") {
                _context.Response.AppendHeader("Vary", "Accept-Encoding");
                _context.Response.AppendHeader("Content-Encoding", "gzip");
                var compressedData = GZipCompress(data);

                File.WriteAllBytes("F:/wtf.gz", compressedData);


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
    }
}
