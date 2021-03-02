using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubversionNet {
    public interface IServer
    {
        string UrlPrefix { get; }
        IRepository GetRepository(string requestUrl);
    }
}
