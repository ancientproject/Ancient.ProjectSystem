namespace Ancient.ProjectSystem
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IExtraction
    {
        Task ExtractTo(DirectoryInfo directory);
    }
}