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

    public class RunePackage : IDisposable, IAsyncDisposable
    {
        public string ID { get; set; }
        public NuGetVersion Version { get; set; }
        public MemoryStream Content { get; set; }
        public RuneSpec Spec { get; set; }

        public void Dispose()
            => Content?.Dispose();
        public ValueTask DisposeAsync()
            => Content?.DisposeAsync() ?? new ValueTask(Task.CompletedTask);

        public static async Task<RunePackage> Unwrap(byte[] buffer, CancellationToken cancellationToken = default)
        {
            await using var mem = new MemoryStream(buffer);
            using var zip = new ZipArchive(mem, ZipArchiveMode.Read);

            var entity = zip.GetEntry("target.rspec");

            if (entity is null)
                throw new CorruptedPackageException($"Could not find target.rspec in package.");

            await using var point = entity.Open();
            await using var memory = new MemoryStream();
            await point.CopyToAsync(memory, cancellationToken);
            var result = Encoding.UTF8.GetString(memory.ToArray());

            var spec = JsonConvert.DeserializeObject<RuneSpec>(result);

            return new RunePackage
            {
                Content = new MemoryStream(buffer),
                ID = spec.ID,
                Version = spec.Version,
                Spec = spec
            };
        }
    }
}