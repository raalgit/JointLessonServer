using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Service.Exceptions;
using JL_Utility;
using JL_Utility.Logger;
using JL_Utility.Models;

namespace JL_Service.Implementation.User
{
    public class LoadNoteAsyncPoint : PointBase<LoadNoteRequest, LoadNoteResponse>, ILoadNoteAsyncPoint
    {
        private readonly IWorkBookRepository _workBookRepository;
        private readonly IFileDataRepository _fileDataRepository;
        private readonly IJLLogger _logger;
        private readonly IFileUtility _fileUtility;

        public LoadNoteAsyncPoint(
            IWorkBookRepository _workBookRepository,
            IFileDataRepository _fileDataRepository,
            IJLLogger _logger,
            IFileUtility _fileUtility,
            ApplicationContext _context) : base(_context)
        {
            this._workBookRepository = _workBookRepository;
            this._fileDataRepository = _fileDataRepository;
            this._fileUtility = _fileUtility;
            this._logger = _logger;
        }

        public override async Task<LoadNoteResponse> Execute(LoadNoteRequest req, UserSettings userSettings)
        {
            var response = new LoadNoteResponse();

            // Получение записи о конспекте курса
            var note = _workBookRepository.Get()
                .FirstOrDefault(x =>  x.UserId == userSettings.User.Id && x.CourseId == req.CourseId)
                ?? throw new PointException("Запись о конспекте не найдена", _logger);

            var fileData = _fileDataRepository.GetById(note.FileDataId);
            response.File = await _fileUtility.GetFileAsBytesById(fileData.MongoId)
                ?? throw new PointException("Файл конспекта не найден", _logger);

            response.Message = $"Конспект получен";
            response.ShowMessage = true;
            return response;
        }
    }
}