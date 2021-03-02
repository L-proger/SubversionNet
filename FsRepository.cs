using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubversionNet {
    public class FsRepository : IRepository{
        public Guid UUID
        {
            get
            {
                if (!File.Exists(UUIDFilePath)) {
                    UUID = Guid.NewGuid();
                    //File.WriteAllText(UUIDFilePath, Guid.NewGuid().ToString());
                }
                return Guid.Parse(File.ReadAllText(UUIDFilePath));
            }
            set => File.WriteAllText(UUIDFilePath, value.ToString());
        }
        public string RootPath => "/svn/TestRepo";
        private string _name;
        public string Name => _name;
        private string _fsPath;

        public string UUIDFilePath => Path.Combine(_fsPath, "UUID.txt");

        public FsRepository(string fsPath)
        {
            _fsPath = fsPath;

            if (!Directory.Exists(fsPath))
            {
                Directory.CreateDirectory(fsPath);
            }

            var uuid = UUID;
            if (!Directory.Exists(GetRepositoryRootPath(0)))
            {
                Directory.CreateDirectory(GetRepositoryRootPath(0));
            }

            _name = Path.GetFileName(fsPath);
        }

        private string GetRepositoryRootPath(int revision)
        {
            return Path.Combine(_fsPath, revision.ToString());
        }

        public IRepositoryRoot GetRepositoryRoot(int revision)
        {
            string repoRootPath = GetRepositoryRootPath(revision);
            if (!Directory.Exists(repoRootPath))
            {
                return null;
            }
            return new FsRepositoryRoot(this, repoRootPath);
        }
    }

    public class FsRepositoryRoot : IRepositoryRoot
    {
        private FsRepository _repository;
        private string _path;
        public FsRepositoryRoot(FsRepository repository, string path)
        {
            _repository = repository;
            _path = path;
        }

        public string GetResourceType(string localUrl)
        {
            var fullLocalPath = Path.Combine(_path, localUrl);
            if (Directory.Exists(fullLocalPath))
            {
                return "collection";
            }
            throw new Exception("Not implemented resource type");
        }
    }
}
