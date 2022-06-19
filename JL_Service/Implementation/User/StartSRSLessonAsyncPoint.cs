using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class StartSRSLessonAsyncPoint : PointBase<StartSRSLessonRequest, StartSRSLessonResponse>, IStartSRSLessonAsyncPoint
    {
        private readonly ILessonRepository _lessonRepository;

        public StartSRSLessonAsyncPoint(
            ILessonRepository _lessonRepository,
            ApplicationContext _context) : base(_context)
        {
            this._lessonRepository = _lessonRepository;
        }

        public override async Task<StartSRSLessonResponse> Execute(StartSRSLessonRequest req, UserSettings userSettings)
        {
            var response = new StartSRSLessonResponse();

            var userId = userSettings.User.Id;
            var srsUserLessons = await _lessonRepository
                .Get()
                .Where(x =>
                    x.TeacherId == userId &&
                    x.Type == PointConsts.LESSON_SRS_TYPE &&
                    x.CourseId == req.CourseId)
                .ToListAsync();

            var activeLesson = srsUserLessons.FirstOrDefault(x => !x.EndDate.HasValue);

            if (activeLesson == null)
            {
                var lastLesson = srsUserLessons.OrderByDescending(x => x.Id).FirstOrDefault();
                var newLesson = new Lesson()
                {
                    CourseId = req.CourseId,
                    StartDate = DateTime.Now,
                    GroupAtCourseId = null,
                    LastMaterialPage = lastLesson != null ? lastLesson.LastMaterialPage : null,
                    EndDate = (DateTime?)null,
                    TeacherId = userId,
                    Type = PointConsts.LESSON_SRS_TYPE
                };
                _lessonRepository.Insert(newLesson);
            }
            else
            {
                response.Page = activeLesson.LastMaterialPage;
            }

            response.Message = "Занятие в режиме СРС начато";
            return response;
        }
    }
}
