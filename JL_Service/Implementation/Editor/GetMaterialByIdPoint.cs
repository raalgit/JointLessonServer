using JL_ApiModels.Response.Editor;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Editor;
using JL_Service.Exceptions;
using JL_Utility.Logger;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.Editor
{
    public class GetMaterialByIdPoint : PointBase<int, GetCourseManualResponse>, IGetMaterialByIdPoint
    {
        private readonly IManualRepository _manualRepository;
        private readonly IJLLogger _logger;

        public GetMaterialByIdPoint(
            IManualRepository _manualRepository,
            IJLLogger _logger,
            ApplicationContext _context) : base(_context)
        {
            this._manualRepository = _manualRepository;
            this._logger = _logger;
        }

        public override async Task<GetCourseManualResponse> Execute(int req, UserSettings userSettings)
        {
            var response = new GetCourseManualResponse();
            response.Manual = await _manualRepository.Get().FirstOrDefaultAsync(x => x.Id == req)
                ?? throw new PointException($"Материал под номером <{req}> не найден", _logger);
            response.Message = $"Материал получен";
            return response;
        }
    }
}
