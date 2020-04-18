namespace Ancient.ProjectSystem
{
    using Newtonsoft.Json;
    using NuGet.Versioning;

    public class AncientLockFile
    {
        public AncientLockFile() {}
        public AncientLockFile(RuneSpec spec)
        {
            id = spec.ID;
            version = spec.Version;
        }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("version"), JsonConverter(typeof(NuGetVersionConverter))]
        public NuGetVersion version { get; set; }
    }
}