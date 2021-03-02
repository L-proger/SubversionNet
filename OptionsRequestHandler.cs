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

        public override void handle(IServer server) {

            var requestData = _context.Request.ReadXml<OptionsRequestData>();

            var repo = server.GetRepository(RequestUrl);
            if (repo == null) {
                throw new Exception("Repository not found!");
            }


      
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
            response.AppendHeader("SVN-Repository-UUID", repo.UUID.ToString());
            response.AppendHeader("SVN-Repository-MergeInfo", "yes");

            response.AppendHeader("DAV", "http://subversion.tigris.org/xmlns/dav/svn/replay-rev-resource");
            response.AppendHeader("SVN-Repository-Root", server.UrlPrefix + "/" + repo.Name);
            response.AppendHeader("SVN-Me-Resource", server.UrlPrefix + "/" + repo.Name + "/!svn/me");
            response.AppendHeader("SVN-Rev-Root-Stub", server.UrlPrefix + "/" + repo.Name + "/!svn/rvr");
            response.AppendHeader("SVN-Rev-Stub", server.UrlPrefix + "/" + repo.Name + "/!svn/rev");
            response.AppendHeader("SVN-Txn-Root-Stub", server.UrlPrefix + "/" + repo.Name + "/!svn/txr");
            response.AppendHeader("SVN-Txn-Stub", server.UrlPrefix + "/" + repo.Name + "/!svn/txn");
            response.AppendHeader("SVN-VTxn-Root-Stub", server.UrlPrefix + "/" + repo.Name + "/!svn/vtxr");
            response.AppendHeader("SVN-VTxn-Stub", server.UrlPrefix + "/" + repo.Name + "/!svn/vtxn");
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
                responseData.ActivityCollectionSet.href = server.UrlPrefix + "/" + repo.Name + "/!svn/act/";
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
