using JL_ApiModels.Request.Editor;
using JL_ApiModels.Response.Editor;
using JL_ManualLib.Models;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Editor;
using JL_Service.Exceptions;
using JL_Utility;
using JL_Utility.Logger;
using JL_Utility.Models;
using System.Text.Json;

namespace JL_Service.Implementation.Editor
{
    public class NewMaterialPoint : PointBase<NewMaterialRequest, NewMaterialResponse>, INewMaterialPoint
    {
        private readonly IFileUtility _fileUtility;
        private readonly IManualRepository _manualRepository;
        private readonly IJLLogger _logger;

        public NewMaterialPoint(
            IFileUtility _fileUtility,
            IJLLogger _logger,
            IManualRepository _manualRepository,
            ApplicationContext _context) : base(_context)
        {
            this._logger = _logger;
            this._fileUtility = _fileUtility;
            this._manualRepository = _manualRepository;
        }

        public override async Task<NewMaterialResponse> Execute(NewMaterialRequest req, UserSettings userSettings)
        {
            var response = new NewMaterialResponse();

            if (req.ManualData == null) throw new PointException("Материал не может быть пустым", _logger);

            var manualJsonBuffer = JsonSerializer.SerializeToUtf8Bytes<ManualData>(req.ManualData);
            Stream stream = new MemoryStream(manualJsonBuffer);
            var fileId = await _fileUtility.CreateNewFileAsync(stream, req.OriginalName, "jl");

            var manual = new Manual()
            {
                AuthorId = userSettings.User.Id,
                FileDataId = fileId,
                Title = req.OriginalName
            };

            _manualRepository.Insert(manual);

            response.Message = $"Новый материл {req.OriginalName} успешно создан. Номер файла {fileId} ";
            return response;
        }
    }
}
