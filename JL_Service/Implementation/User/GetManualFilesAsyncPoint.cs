using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class GetManualFilesAsyncPoint : PointBase<GetManualFilesRequest, GetManualFilesResponse>, IGetManualFilesAsyncPoint
    {
        private readonly IFileDataRepository _fileDataRepository;
        public GetManualFilesAsyncPoint(
            IFileDataRepository _fileDataRepository,
            ApplicationContext _context) : base(_context)
        {
            this._fileDataRepository = _fileDataRepository;
        }

        public override async Task<GetManualFilesResponse> Execute(GetManualFilesRequest req, UserSettings userSettings)
        {
            var response = new GetManualFilesResponse();

            var fileDatas = await _fileDataRepository.Get().Where(x => req.FileDataIds.Distinct().Contains(x.Id)).ToListAsync();
            response.FileDatas = fileDatas;

            response.Message = $"Список файлов материалов получен ({response.FileDatas.Count} шт.)";
            return response;
        }
    }
}
