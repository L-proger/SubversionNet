using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SubversionNet.Protocol {
    [XmlRoot("activity-collection-set", Namespace = WebDav.Namespaces.Dav)]
    public class ActivityCollectionSetData {
        [XmlElement("href", Namespace = WebDav.Namespaces.Dav)] 
        public string href = "/!svn/act/";
    }
}
