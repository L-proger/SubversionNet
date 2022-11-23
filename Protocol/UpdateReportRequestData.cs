using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace SubversionNet.Protocol {

    //[Serializable]
    //[XmlType("allprop", Namespace = WebDav.Namespaces.Dav)]
    //public class AllPropData { }
    //
    [Serializable]
    [XmlType("entry")]
    public class UpdateReportEntry {
        [XmlAttribute("rev")]
        public string Rev = null;

        [XmlAttribute("depth")]
        public string Depth = null;

        [XmlAttribute("linkpath")]
        public string LinkPath = null;

        [XmlAttribute("start-empty")]
        public string StartEmpty = null;

        [XmlAttribute("lock-token")]
        public string LockToken = null;
    }


    //https://github.com/aosm/subversion/blob/e93483d0bc325753517dc54f66e29700d859463a/subversion/subversion/mod_dav_svn/reports/update.c
    [Serializable]
    [XmlRoot("update-report", Namespace = WebDav.Namespaces.Svn)]
    public class UpdateReportRequestData {
        [XmlElement("include-props")]
        public string IncludeProps = null;

        [XmlElement("src-path")]
        public string SrcPath = null;

        [XmlElement("dst-path")]
        public string DstPath = null;

        [XmlElement("update-target")]
        public string UpdateTarget = null;

        [XmlElement("recursive")]
        public string Recursive = null;


        [XmlElement("ignore-ancestry")]
        public string IgnoreAncestry = null;

        [XmlElement("send-copyfrom-args")]
        public string SendCopyfromArgs = null;

        [XmlElement("resource-walk")]
        public string ResourceWalk = null;

        [XmlElement("text-deltas")]
        public string TextDeltas = null;

        [XmlElement("target-revision")]
        public string TargetRevision = null;

        [XmlElement("depth")]
        public string Depth = null;

        [XmlElement("entry")]
        public UpdateReportEntry Entry = null;

        [XmlElement("missing")]
        public string Missing = null;
    }

}
