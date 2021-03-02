using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubversionNet {
    public interface IRepository {
        Guid UUID { get; set; }
        string RootPath { get; }
        string Name { get; }
        IRepositoryRoot GetRepositoryRoot(int revision);
    }

    public interface IRepositoryRoot {
        string GetResourceType(string localUrl);
    }
}