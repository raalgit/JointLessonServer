using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility;
using JL_Utility.Models;

namespace JL_Service.Implementation.User
{
    public class SendNoteAsyncPoint : PointBase<SendNoteRequest, SendNoteResponse>, ISendNoteAsyncPoint
    {
        private readonly IFileUtility _fileUtility;
        private readonly IWorkBookRepository _workBookRepository;

        public SendNoteAsyncPoint(
            IFileUtility _fileUtility,
            IWorkBookRepository _workBookRepository,
            ApplicationContext _context) : base(_context)
        {
            this._fileUtility = _fileUtility;
            this._workBookRepository = _workBookRepository;
        }

        public override async Task<SendNoteResponse> Execute(SendNoteRequest req, UserSettings userSettings)
        {
            var response = new SendNoteResponse();

            Stream stream = new MemoryStream(req.File);
            int fileDataId = await _fileUtility.CreateNewFileAsync(
                stream, 
                $"Конспект_{userSettings.User.Id}_от_{DateTime.Now.ToShortDateString().Replace(" ", "_")}", 
                "rtf");

            // Получение записи о конспекте
            var note = _workBookRepository.Get()
                .FirstOrDefault(x => x.UserId == userSettings.User.Id && x.CourseId == req.CourseId);
            
            if (note != null)
            {
                note.FileDataId = fileDataId;
                _workBookRepository.Update(note);
            }
            else
            {
                var newNote = new WorkBook()
                {
                    FileDataId = fileDataId,
                    CourseId = req.CourseId,
                    UserId = userSettings.User.Id
                };
                _workBookRepository.Insert(newNote);
            }
            response.Message = $"Конспект обновлен";
            response.ShowMessage = true;
            return response;
        }
    }
}
