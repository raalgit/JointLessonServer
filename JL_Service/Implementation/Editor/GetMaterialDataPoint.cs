using JL_ApiModels.Response.Editor;
using JL_ManualLib.Models;
using JL_MSSQLServer;
using JL_Service.Abstraction.Editor;
using JL_Service.Exceptions;
using JL_Utility;
using JL_Utility.Logger;
using JL_Utility.Models;
using System.Text.Json;

namespace JL_Service.Implementation.Editor
{
    public class GetMaterialDataPoint : PointBase<int, GetMaterialResponse>, IGetMaterialDataPoint
    {
        private readonly IFileUtility _fileUtility;
        private readonly IJLLogger _logger;

        public GetMaterialDataPoint(
            IFileUtility _fileUtility,
            IJLLogger _logger,
            ApplicationContext _context) : base(_context)
        {
            this._fileUtility = _fileUtility;
            this._logger = _logger;
        }

        public override async Task<GetMaterialResponse> Execute(int req, UserSettings userSettings)
        {
            var response = new GetMaterialResponse();

            var fileData = _fileUtility.GetFileData(req)
                ?? throw new PointException($"Файл с номером <{req}> не найден", _logger);

            var fileBytes = await _fileUtility.GetFileAsBytesById(fileData.MongoId)
                ?? throw new PointException($"не удалось получить данные файла <{req}>", _logger);

            using (Stream stream = new MemoryStream(fileBytes))
            {
                try
                {
                    var manual = await JsonSerializer.DeserializeAsync<ManualData>(stream);
                    response.ManualData = manual;
                }
                catch (Exception ex)
                {
                    throw new PointException($"не удалось десериализовать файл <{req}>", _logger);
                }
            }
            
            response.Message = $"Данные материала {fileData.OriginalName} получены";
            return response;
        }
    }
}
