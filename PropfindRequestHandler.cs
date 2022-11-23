using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SubversionNet.Protocol;

namespace SubversionNet {
    public class PropfindRequestHandler : RequestHandler {
        public PropfindRequestHandler(HttpListenerContext context) : base(context) {
        }

        public override void handle(IServer server) {
            var repo = server.GetRepository(RequestUrl);
            if (repo == null) {
                throw new Exception("Repository not found!");
            }

            var requestData = _context.Request.ReadXml<PropFindRequestData>();

            var localPath = GetLocalRepoUrl(server, repo);


            StringBuilder result = new StringBuilder();
            result.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            result.Append("<D:multistatus xmlns:D=\"DAV:\" xmlns:ns0=\"DAV:\">\n");
            result.Append("<D:response xmlns:lp1=\"DAV:\">\n");

            var resultHref = RequestUrl;
            if (!resultHref.EndsWith('/'))
            {
                resultHref = resultHref + "/";
            }

            result.Append($"<D:href>{resultHref}</D:href>\n");
            result.Append($"<D:propstat>\n");
            result.Append($"<D:prop>\n");


            if (localPath.StartsWith(Protocol.Constants.Paths.RevisionRootResource))
            {
                var rvrArgsString = localPath.Substring(Protocol.Constants.Paths.RevisionRootResource.Length + 1);

                var args = rvrArgsString.Split('/', 2);
                var rev = args[0];
                var path = args.Length > 1 ? args[1] : "";

                var repoRoot = repo.GetRepositoryRoot(int.Parse(rev));

                if (requestData.Prop != null)
                {
                    foreach (var p in requestData.Prop.Properties)
                    {
                        if (p.Name == "resourcetype")
                        {
                            var t = repoRoot.GetResourceType(path);
                            result.Append($"<lp1:resourcetype><D:{t}/></lp1:resourcetype>\n");
                        }
                        else
                        {
                            throw new Exception("unknown property");
                        }
                    }

                    
                }
            }


            result.Append($"</D:prop>\n");
            result.Append($"<D:status>HTTP/1.1 200 OK</D:status>\n");
            result.Append($"</D:propstat>\n");

            result.Append("</D:response>\n");
            result.Append("</D:multistatus>\n");

            var resultStr = result.ToString();
            var resultBytes = Encoding.UTF8.GetBytes(resultStr);


            _context.Response.ContentType = "text/xml; charset=\"utf-8\"";
            _context.Response.ContentEncoding = null;
            _context.Response.StatusCode = 207;

            if (_context.Request.Headers["Label"] != null) {
                _context.Response.AppendHeader("Vary", "Label");
            }

            WriteResponse(resultBytes);

        }
    }
}