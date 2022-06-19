using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class ChangeActivePageAsyncPoint : PointBase<ChangeSRSLessonManualPageRequest, ChangeSRSLessonManualPageResponse>, IChangeActivePageAsyncPoint
    {
        private readonly ILessonRepository _lessonRepository;
        public ChangeActivePageAsyncPoint(
            ILessonRepository _lessonRepository,
            ApplicationContext _context) : base(_context)
        {
            this._lessonRepository = _lessonRepository;
        }

        public override async Task<ChangeSRSLessonManualPageResponse> Execute(ChangeSRSLessonManualPageRequest req, UserSettings userSettings)
        {
            var response = new ChangeSRSLessonManualPageResponse();

            var userId = userSettings.User.Id;
            var activeLesson = await _lessonRepository.Get()
                .FirstOrDefaultAsync(x =>
                    x.TeacherId == userId &&
                    x.Type == PointConsts.LESSON_SRS_TYPE &&
                    x.CourseId == req.CourseId &&
                    !x.EndDate.HasValue
                    );

            if (activeLesson != null)
            {
                activeLesson.LastMaterialPage = req.NextPage;
            }

            response.Message = $"Активная страница изменена на {req.NextPage}";
            return response;
        }
    }
}
