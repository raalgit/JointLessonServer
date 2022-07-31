using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JL_MongoDB.Repository
{
    public interface IMongoRepository
    {
        public string GetNewFileName(string fileExtension);
        public Task<byte[]> GetFileBytesByIdAsync(string mongoId);
        public Task<string> UploadFileAsync(Stream fileStream, string fileName);
        public Task<Stream> DownloadFileByIdAsync(string mongoId);
        public Task<bool> TryDeleteFileAsync(string mongoId);
        public Task<string> ChangeFileAsync(string mongoId, Stream newFileStream, string newFileName);
    }
}
