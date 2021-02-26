using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SubversionNet.Protocol;

namespace SubversionNet {
    class OptionsRequestHandler : RequestHandler {
        public OptionsRequestHandler(HttpListenerContext context) : base(context) {

        }

        public override void handle() {

            var requestData = _context.Request.ReadXml<OptionsRequestData>();


            var acceptEncoding = _context.Request.Headers["Accept-Encoding"];

            if (requestData == null) {
                throw new Exception("Data is null");
            }

            var response = _context.Response;
            response.AppendHeader("DAV", "1,2");
            response.AppendHeader("DAV", "version-control,checkout,working-resource");
            response.AppendHeader("DAV", "merge,baseline,activity,version-controlled-collection");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/depth");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/log-revprops");

            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/atomic-revprops");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/partial-replay");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/inherited-props");

            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/inline-props");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/reverse-file-revs");

            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/list");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/mergeinfo");

   

            response.AppendHeader("MS-Author-Via", "DAV");
            response.AppendHeader("Allow", "OPTIONS,GET,HEAD,POST,DELETE,TRACE,PROPFIND,PROPPATCH,COPY,MOVE,LOCK,UNLOCK,CHECKOUT");

            response.AppendHeader("SVN-Youngest-Rev", "0");
            response.AppendHeader("SVN-Repository-UUID", "42eb9918-f851-4e6b-8a11-97df4a0a8802");
            response.AppendHeader("SVN-Repository-MergeInfo", "yes");

            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/replay-rev-resource");
            response.AppendHeader("SVN-Repository-Root", "/svn/TestRepo");
            response.AppendHeader("SVN-Me-Resource", "/svn/TestRepo/!svn/me");
            response.AppendHeader("SVN-Rev-Root-Stub", "/svn/TestRepo/!svn/rvr");
            response.AppendHeader("SVN-Rev-Stub", "/svn/TestRepo/!svn/rev");
            response.AppendHeader("SVN-Txn-Root-Stub", "/svn/TestRepo/!svn/txr");
            response.AppendHeader("SVN-Txn-Stub", "/svn/TestRepo/!svn/txn");
            response.AppendHeader("SVN-VTxn-Root-Stub", "/svn/TestRepo/!svn/vtxr");
            response.AppendHeader("SVN-VTxn-Stub", "/svn/TestRepo/!svn/vtxn");
            response.AppendHeader("SVN-Allow-Bulk-Updates", "On");
            response.AppendHeader("SVN-Supported-Posts", "create-txn");
            response.AppendHeader("SVN-Supported-Posts", "create-txn-with-props");


            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/ephemeral-txnprops");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/svndiff1");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/svndiff2");
            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/put-result-checksum");

            response.AppendHeader("Keep-Alive", "timeout=5, max=100");
            response.AppendHeader("Connection", "Keep-Alive");


            response.ContentType = "text/xml; charset=\"utf-8\"";
            response.ContentEncoding = null;
            response.StatusCode = 200;


            var responseData = new OptionsResponseData();




            if (requestData.ActivityCollectionSet != null) {
                responseData.ActivityCollectionSet = new ActivityCollectionSetData();
                responseData.ActivityCollectionSet.href = "/svn/TestRepo/!svn/act/";
            }


           /* MemoryStream ms = new MemoryStream();
            using (StreamWriter output = new StreamWriter(ms)) {
                output.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
                output.Write("<D:options-response xmlns:D=\"DAV:\">\n");
                if (requestData.ActivityCollectionSet != null)
                {
                    output.Write("<D:activity-collection-set><D:href>/svn/TestRepo/!svn/act/</D:href></D:activity-collection-set></D:options-response>\n");
                }
                else
                {
                    output.Write("</D:options-response>\n");
                }
            }
            WriteResponse(ms.ToArray());*/
            
           WriteResponse(XmlUtils.Serialize(responseData));


            // using (StreamWriter output = new StreamWriter(response.OutputStream))
            // {

            //  output.wr

            /* output.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
             output.Write("<D:options-response xmlns:D=\"DAV:\">\n");
             output.Write("<D:activity-collection-set><D:href>" + ("/svn/TestRepo/!svn/act/") + "</D:href></D:activity-collection-set></D:options-response>\n");*/
            //}

        }
    }
}
