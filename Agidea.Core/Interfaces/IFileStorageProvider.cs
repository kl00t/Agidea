namespace Agidea.Core.Interfaces
{
    public interface IFileStorageProvider
    {
        void GetFile(string fileName);

        void ListFiles();
    }
}