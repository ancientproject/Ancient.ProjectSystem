namespace Ancient.ProjectSystem
{
    using System;
    using Newtonsoft.Json;
    using NuGet.Versioning;

    public class NuGetVersionConverter : JsonConverter<NuGetVersion>
    {
        public override void WriteJson(JsonWriter writer, NuGetVersion value, JsonSerializer serializer) => writer.WriteValue(value.ToString());

        public override NuGetVersion ReadJson(JsonReader reader, Type objectType, NuGetVersion existingValue, bool hasExistingValue,
            JsonSerializer serializer) =>
            NuGetVersion.Parse((string)reader.Value);
    }
}