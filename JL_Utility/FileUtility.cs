using JL_MongoDB.Repository;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;

namespace JL_Utility
{
    public class FileUtility : IFileUtility
    {
        private readonly IMongoRepository _mongoRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly ApplicationContext _context;

        public FileUtility(IMongoRepository mongoRepository,
                           IFileDataRepository fileDataRepository,
                           ApplicationContext context)
        {
            _mongoRepository = mongoRepository ?? throw new NullReferenceException(nameof(mongoRepository));
            _fileDataRepository = fileDataRepository ?? throw new NullReferenceException(nameof(fileDataRepository));
            _context = context;
        }

        public async Task<int> CreateNewFileAsync(Stream fileStream, string originalFileName, string fileExtension)
        {
            var originalFile = _fileDataRepository.Get().FirstOrDefault(x => x.OriginalName == originalFileName);
            if (originalFile != null)
            {
                return originalFile.Id;
            }

            string mongoName = _mongoRepository.GetNewFileName(fileExtension);

            var file = new FileData();
            file.MongoName = mongoName;
            file.OriginalName = originalFileName;

            var mongoId = await _mongoRepository.UploadFileAsync(fileStream, mongoName);
            file.MongoId = mongoId;

            file = _fileDataRepository.Insert(file);

            _context.SaveChanges();

            return file.Id;
        }

        public async Task<int> UpdateFileAsync(Stream fileStream, string mongoId, string originalFileName, string fileExtension)
        {
            string mongoName = _mongoRepository.GetNewFileName(fileExtension);

            var updatedFile = new FileData();
            updatedFile.MongoName = mongoName;
            updatedFile.OriginalName = originalFileName;

            var newMongoId = await _mongoRepository.ChangeFileAsync(mongoId, fileStream, mongoName);
            updatedFile.MongoId = newMongoId;

            updatedFile = _fileDataRepository.Insert(updatedFile);
            _context.SaveChanges();

            return updatedFile.Id;
        }

        public async Task<byte[]> GetFileAsBytesById(string mongoId)
        {
            return await _mongoRepository.GetFileBytesByIdAsync(mongoId);
        }

        public FileData GetFileData(int fileId)
        {
            return _fileDataRepository.GetById(fileId) ?? throw new NullReferenceException();
        }

        public async Task<int> ClearOldRecords()
        {
            return 0;
        }
    }
}
