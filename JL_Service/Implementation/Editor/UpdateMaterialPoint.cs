using JL_ApiModels.Request.Editor;
using JL_ApiModels.Response.Editor;
using JL_ManualLib.Models;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Editor;
using JL_Service.Exceptions;
using JL_Utility;
using JL_Utility.Logger;
using JL_Utility.Models;
using System.Text.Json;

namespace JL_Service.Implementation.Editor
{
    public class UpdateMaterialPoint : PointBase<UpdateMaterialRequest, UpdateMaterialResponse>, IUpdateMaterialPoint
    {
        private readonly IFileUtility _fileUtility;
        private readonly IManualRepository _manualRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly IJLLogger _logger;

        public UpdateMaterialPoint(
            IFileUtility _fileUtility,
            IJLLogger _logger,
            IFileDataRepository _fileDataRepository,
            IManualRepository _manualRepository,
            ApplicationContext _context) : base(_context)
        {
            this._fileUtility = _fileUtility;
            this._manualRepository = _manualRepository;
            this._fileDataRepository = _fileDataRepository;
            this._logger = _logger;
        }

        public override async Task<UpdateMaterialResponse> Execute(UpdateMaterialRequest req, UserSettings userSettings)
        {
            var response = new UpdateMaterialResponse();

            if (req.ManualData == null) throw new PointException("Материал не может быть пустым", _logger);

            Stream stream = new MemoryStream(req.ManualData);

            var originalFileData = _fileDataRepository.GetById(req.OriginalFileDataId) 
                ?? throw new PointException($"Не удалось найти файл по номеру <{req.OriginalFileDataId}>", _logger);

            var updatedFileId = await _fileUtility.UpdateFileAsync(stream, originalFileData.MongoId, req.OriginalName, ".jl");
            var fileData = _manualRepository.Get().Where(x => x.FileDataId == req.OriginalFileDataId).FirstOrDefault()
                ?? throw new PointException($"Оригинальный материал не найден по номеру <{req.OriginalFileDataId}>", _logger);
            
            fileData.FileDataId = updatedFileId;
            _manualRepository.Update(fileData);
            response.Message = $"Материал {fileData.Title} успешно обновлен. Новый номер файла {fileData.FileDataId}";

            stream.Dispose();
            return response;
        }
    }
}
