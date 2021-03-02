using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubversionNet {
    public class FsServer : IServer
    {
        private string _urlPrefix;
        public string UrlPrefix => _urlPrefix;
        private string _rootDirectory;


        public FsServer(string rootDirectory, string urlPrefix)
        {
            _rootDirectory = rootDirectory;
            _urlPrefix = urlPrefix;
        }

        public IRepository GetRepository(string requestUrl)
        {
            if (!requestUrl.StartsWith(UrlPrefix))
            {
                Console.WriteLine($"Requested URL {requestUrl} is not on this server {UrlPrefix}");
                return null;
            }

            var localUrl = requestUrl.Substring(_urlPrefix.Length + 1);
            var repoName = localUrl.Split('/', 2)[0];

            if (Directory.Exists(GetRepositoryPath(repoName)))
            {
                return new FsRepository(GetRepositoryPath(repoName));
            }

            Console.WriteLine($"No such repository {requestUrl} on server {UrlPrefix}");
            return null;
        }

        private string GetRepositoryPath(string name)
        {
            return Path.Combine(_rootDirectory, name);
        }

        public FsRepository CreateRepository(string name)
        {
            return new FsRepository(GetRepositoryPath(name));
        }
    }
}
