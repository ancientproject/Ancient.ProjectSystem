namespace Ancient.ProjectSystem
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using exceptions;
    using Newtonsoft.Json;
    using NuGet.Versioning;

    public class RunePackage : IDisposable, IAsyncDisposable, IExtraction
    {
        public string ID { get; set; }
        public NuGetVersion Version { get; set; }
        public MemoryStream Content { get; set; }
        public RuneSpec Spec { get; set; }

        public void Dispose()
            => Content?.Dispose();
        public ValueTask DisposeAsync()
            => Content?.DisposeAsync() ?? new ValueTask(Task.CompletedTask);
        public static async Task<RunePackage> Unwrap(Stream stream, CancellationToken cancellationToken = default)
        {
            using var zip = new ZipArchive(stream);
            var entity = zip.GetEntry("target.rspec");

            if (entity is null)
                throw new CorruptedPackageException($"Could not find target.rspec in package.");

            await using var point = entity.Open();
            await using var memory = new MemoryStream();
            await point.CopyToAsync(memory, cancellationToken);
            var result = Encoding.UTF8.GetString(memory.ToArray());

            var spec = JsonConvert.DeserializeObject<RuneSpec>(result);
            var content = new MemoryStream();
            await stream.CopyToAsync(content, cancellationToken);

            return new RunePackage
            {
                Content = content,
                ID = spec.ID,
                Version = spec.Version,
                Spec = spec
            };
        }

        async Task IExtraction.ExtractTo(DirectoryInfo directory)
        {
            using var zip = new ZipArchive(Content);

            foreach (var entry in zip.Entries)
            {
                var path = Path.Combine(directory.FullName, entry.FullName);
                await using var point = File.OpenWrite(path);
                await using var entryPoint = entry.Open();
                await entryPoint.CopyToAsync(point);
                await point.FlushAsync();
            }
        }
    }
}