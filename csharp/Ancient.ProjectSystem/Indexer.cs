namespace Ancient.ProjectSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;
    using MoreLinq;
    using NuGet.Versioning;

    /// <summary>
    /// (not thread-safe)
    /// </summary>
    public class Indexer
    {
        internal DirectoryInfo _path { get; set; }
        internal bool useLockFile { get; set; }

        internal List<AncientLockFile> lockFile { get; set; }
        internal FileInfo lockFileRef { get; set; }

        public Indexer(string path)
        {
            _path = new DirectoryInfo(path);
            if (!_path.Exists)
                Directory.CreateDirectory(path);
            _path = new DirectoryInfo(path);
        }
        public static Indexer FromLocal() => new Indexer("./deps");

        public Indexer UseLock()
        {
            if (!_path.Exists)
                Directory.CreateDirectory(_path.FullName);
            useLockFile = true;
            lockFile = new List<AncientLockFile>();
            lockFileRef = new FileInfo(Path.Combine(_path.FullName, "ancient.lock"));
            if(!lockFileRef.Exists)
                FlushLock();
            LoadLock();
            return this;
        }

        public Indexer DropDeps()
        {
            _path.GetFiles("*.*").Pipe(x => x.Delete()).ToArray();
            _path.GetDirectories().Pipe(x => x.Delete(true)).ToArray();
            return UseLock();
        }

        public Indexer SaveDep(Assembly asm, byte[] assemblyBytes, RuneSpec spec)
        {
            var depDir = _path
                .CreateSubdirectory(spec.ID)
                .CreateSubdirectory($"{spec.Version}")
                .CreateSubdirectory("any");

            File.WriteAllBytes(Path.Combine(depDir.FullName, $"{asm.GetName().Name}.image"), assemblyBytes);

            if (useLockFile)
            {
                lockFile.Add(new AncientLockFile(spec));
                FlushLock();
            }
            return this;
        }

        public Indexer RevDep(string id)
        {
            new DirectoryInfo(Path.Combine(_path.FullName, $"{Path.GetFileNameWithoutExtension(id)}"))
                .GetFiles("*.*").Pipe(x => x.Delete()).ToArray();
            new DirectoryInfo(Path.Combine(_path.FullName, $"{Path.GetFileNameWithoutExtension(id)}")).Delete(true);
            if (useLockFile)
            {
                lockFile.Remove(lockFile.First(x => x.id == id));
                FlushLock();
            }
            return this;
        }

        public Indexer GetVersion(string id, out NuGetVersion version)
        {
            version = lockFile.First(x => x.id == id).version;
            return this;
        }

        public bool Exist(string id) => lockFile.FirstOrDefault(x => x.id == id) != null;


        private void FlushLock() => File.WriteAllText(lockFileRef.FullName, JsonConvert.SerializeObject(lockFile));
        private void LoadLock() => lockFile = JsonConvert.DeserializeObject<List<AncientLockFile>>(File.ReadAllText(lockFileRef.FullName));
    }
}