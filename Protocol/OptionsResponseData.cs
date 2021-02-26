using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SubversionNet.Protocol {
    [XmlRoot("options-response", Namespace = WebDav.Namespaces.Dav)]
    public class OptionsResponseData {
        [XmlElement("activity-collection-set", Namespace = WebDav.Namespaces.Dav)]
        public ActivityCollectionSetData ActivityCollectionSet = null;
    }
}
