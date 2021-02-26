using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SubversionNet.Protocol {

    [Serializable]
    [XmlType("allprop", Namespace = WebDav.Namespaces.Dav)]
    public class AllPropData { }

    [Serializable]
    [XmlType("propname", Namespace = WebDav.Namespaces.Dav)]
    public class PropNameData { }

    [Serializable]
    public class PropData {
        [XmlAnyElement()]
        public List<XmlElement> Properties = new List<XmlElement>();
    }

    [Serializable]
    [XmlRoot("propfind", Namespace = WebDav.Namespaces.Dav)]
    public class PropFindData {
        [XmlElement("prop", Namespace = WebDav.Namespaces.Dav)] 
        public PropData Prop = null;
        [XmlElement("allprop", Namespace = WebDav.Namespaces.Dav)] 
        public AllPropData AllProp = null;
        [XmlElement("propname", Namespace = WebDav.Namespaces.Dav)] 
        public PropNameData PropName = null;
    }
}
