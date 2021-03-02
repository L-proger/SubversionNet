using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubversionNet.Protocol.Constants {
    //https://svn.apache.org/repos/asf/subversion/trunk/notes/http-and-webdav/http-protocol-v2.txt
    public static class Paths
    {
        public static class Deprecated
        {
            public static readonly string VersionControlledResource = "!svn/vcc";
            public static readonly string BaselineResource = "!svn/bln";
            public static readonly string WorkingBaselineResource = "!svn/wbl";
            public static readonly string BaselineCollectionResource = "!svn/bc"; // !svn/bc/REV/
            public static readonly string ActivityCollection = "!svn/act"; // !svn/act/ACTIVITY-UUID/
            public static readonly string VersionedResource = "!svn/ver"; // !svn/ver/REV/path
            public static readonly string WorkingResource = "!svn/wrk"; // !svn/wrk/ACTIVITY-UUID/path
        }
      

        //Represents the "repository itself".  This is the URI that custom REPORTS are sent against.
        //(This eliminates our need for the VCC resource.)
        public static readonly string MeResource = "!svn/me"; // !svn/wrk/ACTIVITY-UUID/path

        /*  Represents a Subversion revision at the metadata level, and
            maps conceptually to a "revision" in the FS layer.  Standard
            PROPFIND and PROPATCH requests can be used against a revision
            resource, with the understanding that the name/value pairs
            being accessed are unversioned revision props, rather than file
            or directory props.  (This eliminates our need for baseline or
            working baseline resources.)
        */
        public static readonly string RevisionResource = "!svn/rev"; //!svn/rev/REV

        /*  Represents the directory tree snapshot associated with a
            Subversion revision, and maps conceptually to a revision-type
            svn_fs_root_t/path pair in the FS layer.  GET, PROPFIND, and
            certain REPORT requests can be issued against these resources.
        */
        public static readonly string RevisionRootResource = "!svn/rvr"; //!svn/rvr/REV/[PATH]

        /*  Represents a Subversion commit transaction, and maps
            conceptually to an svn_fs_txn_t in the FS layer.  PROPFIND and
            PROPATCH requests can be used against a transaction resource,
            with the understanding that the name/value pairs being accessed
            are unversioned transaction props, rather than file or directory
            props.
        */
        public static readonly string TransactionResource = "!svn/txn";  //(!svn/txn/TXN-NAME)

        /*  Represents the directory tree snapshot associated with a
            Subversion commit transaction, and maps conceptually to a
            transaction-type svn_fs_root_t/path pair in the FS layer.
            Various read- and write-type requests can be issued against
            these resources (MKCOL, PUT, PROPFIND, PROPPATCH, GET, etc.).
        */
        public static readonly string TransactionRootResource = "!svn/txr"; //"!svn/txr/TXN-NAME/[PATH])


        /*  Alternative names for the transaction based on a virtual, or
            visible, name supplied by the client when the transaction
            was created.  The client supplied name is optional, if not
            supplied these resource names are not valid.
        */
        public static readonly string AlternateTransactionResource = "!svn/vtxn"; //!svn/vtxn/VTXN-NAME
        public static readonly string AlternateTransactionRootResource = "!svn/vtxr"; //!svn/vtxr/VTXN-NAME/[PATH]
    }
}
