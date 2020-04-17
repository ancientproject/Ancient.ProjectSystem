namespace Ancient.ProjectSystem
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using NuGet.Versioning;

    public class RuneSpec
    {
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("version")]
        public NuGetVersion Version { get; set; }
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType Type { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("files")]
        public IList<string> Files { get; set; }
        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }
        [JsonProperty("license")]
        public string License { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("repository")]
        public RepositoryBlob Repository { get; set; }

        public class RepositoryBlob
        {
            public string type { get; set; }
            public string url { get; set; }
        }

        public enum PackageType
        {
            Binary,
            Source
        }
    }
}