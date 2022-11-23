using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SubversionNet.Protocol;

namespace SubversionNet {
    public static class HttpListenerRequestExtensions {
        public static T ReadXml<T>(this HttpListenerRequest request) where T : class
        {
            if (request.ContentLength64 == 0)
            {
                return null;
            }
            var readerSettings = new XmlReaderSettings { CloseInput = false };
            using var reader = XmlReader.Create(request.InputStream, readerSettings);
            reader.MoveToContent();
            return XmlUtils.Deserialize<T>(reader);
        }

        public static string ReadText(this HttpListenerRequest request)  {
            if (request.ContentLength64 == 0) { 
                return null;
            }

            var reader = new System.IO.StreamReader(request.InputStream);
            var result = reader.ReadToEnd();
            reader.Close();
            return result;

        }
    }
}
