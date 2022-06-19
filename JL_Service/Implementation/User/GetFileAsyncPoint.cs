using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility;
using JL_Utility.Models;

namespace JL_Service.Implementation.User
{
    public class GetFileAsyncPoint : PointBase<int, GetFileResponse>, IGetFileAsyncPoint
    {
        private readonly IFileDataRepository _fileDataRepository;
        private readonly IFileUtility _fileUtility;
        public GetFileAsyncPoint(
            IFileDataRepository _fileDataRepository,
            IFileUtility _fileUtility,
            ApplicationContext _context) : base(_context)
        {
            this._fileDataRepository = _fileDataRepository;
            this._fileUtility = _fileUtility;
        }

        public override async Task<GetFileResponse> Execute(int req, UserSettings userSettings)
        {
            var response = new GetFileResponse();

            var fileData = _fileDataRepository.GetById(req);
            response.File = await _fileUtility.GetFileAsBytesById(fileData.MongoId);

            response.Message = $"Файл под номером {req} получен";
            return response;
        }
    }
}
