using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_Service.Abstraction.User;
using JL_Utility;
using JL_Utility.Models;

namespace JL_Service.Implementation.User
{
    public class AddNewFileAsyncPoint : PointBase<AddNewFileRequest, AddNewFileResponse>, IAddNewFileAsyncPoint
    {
        private readonly IFileUtility _fileUtility;
        public AddNewFileAsyncPoint(
            IFileUtility _fileUtility,
            ApplicationContext _context) : base(_context)
        {
            this._fileUtility = _fileUtility;
        }

        public override async Task<AddNewFileResponse> Execute(AddNewFileRequest req, UserSettings userSettings)
        {
            var response = new AddNewFileResponse();

            req.Name = req.Name.Split('\\').LastOrDefault() ??
                throw new Exception("Ошибка в названии файла");

            string extension = Path.GetExtension(req.Name);
            Stream stream = new MemoryStream(req.File);
            int fileDataId = await _fileUtility.CreateNewFileAsync(stream, req.Name, extension);
            response.FileDataId = fileDataId;

            response.Message = $"Файл {req.Name} успешно загружен под номером {fileDataId}";
            return response;
        }
    }
}
