using JL_MSSQLServer.PersistModels;

namespace JL_Utility
{
    public interface IFileUtility
    {
        public Task<int> CreateNewFileAsync(Stream fileStream, string originalFileName, string fileExtension);
        public Task<int> UpdateFileAsync(Stream fileStream, string mongoId, string originalFileName, string fileExtension);
        public Task<byte[]> GetFileAsBytesById(string mongoId);
        public FileData GetFileData(int fileId);
        public Task<int> ClearOldRecords();
    }
}
