using JL_ApiModels.Response.Editor;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Editor;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.Editor
{
    public class GetMyMaterialsPoint : PointBase<object?, GetMyMaterialsResponse>, IGetMyMaterialsPoint
    {
        private readonly IManualRepository _manualRepository;

        public GetMyMaterialsPoint(
            IManualRepository _manualRepository,
            ApplicationContext _context) : base(_context)
        {
            this._manualRepository = _manualRepository;
        }

        public override async Task<GetMyMaterialsResponse> Execute(object? req, UserSettings userSettings)
        {
            var response = new GetMyMaterialsResponse();

            response.Manuals = await _manualRepository.Get().Where(x => x.AuthorId == userSettings.User.Id).ToListAsync();
            response.Message = $"Список материалов получен ({response.Manuals.Count} шт.)";

            return response;
        }
    }
}
