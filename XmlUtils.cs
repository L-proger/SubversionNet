using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SubversionNet {
    public static class XmlUtils {

        public static T Deserialize<T>(XmlReader reader) {
            var requestSerializer = new XmlSerializer(typeof(T));
            return (T)requestSerializer.Deserialize(reader);
        }

        public static T Deserialize<T>(string xml) {
            var readerSettings = new XmlReaderSettings {CloseInput = false};
            return Deserialize<T>(XmlReader.Create(new StringReader(xml), readerSettings));
        }

        public static byte[] Serialize<T>(T data) {
            var settings = new XmlWriterSettings {
                CloseOutput = false,
                Encoding = new System.Text.UTF8Encoding(false)
                
            };
            var xml = new MemoryStream();
            var writer = XmlWriter.Create(xml, settings);
            var serializer = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            serializer.Serialize(writer, data, ns);
            writer.Flush();
            return xml.ToArray();
        }

	}
}
