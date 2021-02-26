using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SubversionNet.Protocol;

namespace SubversionNet {
    class PropfindRequestHandler : RequestHandler {
        public PropfindRequestHandler(HttpListenerContext context) : base(context) {
        }

        public override void handle() {
            var requestData = _context.Request.ReadXml<PropFindRequestData>();

            throw new Exception("Not implemented");
        }
    }
}