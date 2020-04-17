namespace Ancient.ProjectSystem
{
    using System;
    using System.IO;
    using System.Reflection;

    public class AncientLockFile
    {
        public AncientLockFile() {}
        public AncientLockFile(Assembly asm, string Registry)
        {
            id = Path.GetFileNameWithoutExtension(asm.GetName().Name);
            registry = Registry;
            version = asm.GetName().Version;
        }

        public string id { get; set; }
        public string registry { get; set; }
        public Version version { get; set; }
        public string platform { get; set; } = "any";
    }
}