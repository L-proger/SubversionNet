using SubversionNet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubversionNet {
    public class ReportRequestHandler : RequestHandler {
        public ReportRequestHandler(HttpListenerContext context) : base(context) {

        }
        
        static bool IsRootElementNameMatch(string xmlrequest, string name) {
            return Regex.Match(xmlrequest, $"(?s)<[^\\/]*{name}[^>]*>").Success;
        }

        public override void handle(IServer server) {
            var requestString = _context.Request.ReadText();

            Console.WriteLine("~~~ Request body:");
            Console.WriteLine(requestString);
            Console.WriteLine();

            if (IsRootElementNameMatch(requestString, "update-report")) {
                handleUpdateReport(server, requestString);
            } else if (IsRootElementNameMatch(requestString, "log-report")) {
                throw new NotImplementedException();
            } else if (IsRootElementNameMatch(requestString, "dated-rev-report")) {
                throw new NotImplementedException();
            } else if (IsRootElementNameMatch(requestString, "get-location-segments")) {
                throw new NotImplementedException();
            } else if (IsRootElementNameMatch(requestString, "get-locations")) {
                throw new NotImplementedException();
            } else if (IsRootElementNameMatch(requestString, "file-revs-report")) {
                throw new NotImplementedException();
            } else if (IsRootElementNameMatch(requestString, "get-locks-report")) {
                throw new NotImplementedException();
            } else if (IsRootElementNameMatch(requestString, "replay-report")) {
                throw new NotImplementedException();
            } else if (IsRootElementNameMatch(requestString, "get-deleted-rev-report")) {
                throw new NotImplementedException();
            } else {
                throw new Exception("Unknown report requested");
            }
        }

        private void handleUpdateReport(IServer server, string requestString) {

            var request = ParseXml<UpdateReportRequestData>(requestString);

            if (string.IsNullOrEmpty(request.SrcPath)) {
                WriteBadRequestResponse("The request did not contain the '<src-path>' element.\nThis may indicate that your client is too old.");
                return;
            }

            if (string.IsNullOrEmpty(request.TargetRevision)) {
                //find HEAD revision
                throw new NotImplementedException();
            }


            //switch operation
            if (!string.IsNullOrEmpty(request.DstPath)) {

            }


            throw new NotImplementedException();
        }

    }

}
