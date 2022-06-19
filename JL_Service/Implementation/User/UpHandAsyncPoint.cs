using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_SignalR;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class UpHandAsyncPoint : PointBase<UpHandRequest, UpHandResponse>, IUpHandAsyncPoint
    {
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILessonTabelRepository _lessonTabelRepository;
        private readonly ISignalRUtility _signalRUtility;
        private readonly IUserGroupRepository _userGroupRepository;

        public UpHandAsyncPoint(
            IGroupAtCourseRepository _groupAtCourseRepository,
            ILessonRepository _lessonRepository,
            ISignalRUtility _signalRUtility,
            IUserGroupRepository _userGroupRepository,
            ILessonTabelRepository _lessonTabelRepository,
            ApplicationContext _context) : base(_context)
        {
            this._signalRUtility = _signalRUtility;
            this._userGroupRepository = _userGroupRepository;
            this._groupAtCourseRepository = _groupAtCourseRepository;
            this._lessonRepository = _lessonRepository;
            this._lessonTabelRepository = _lessonTabelRepository;
        }

        public override async Task<UpHandResponse> Execute(UpHandRequest req, UserSettings userSettings)
        {
            var response = new UpHandResponse();

            var getUserLessonTabelsQuery = from userGroup in _userGroupRepository.Get()
                                           join groupAtCourse in _groupAtCourseRepository.Get()
                                           on userGroup.GroupId equals groupAtCourse.GroupId
                                           join lesson in _lessonRepository.Get()
                                           on userGroup.Id equals lesson.GroupAtCourseId
                                           join userTabel in _lessonTabelRepository.Get()
                                           on lesson.Id equals userTabel.LessonId
                                           where
                                           userGroup.UserId == userSettings.User.Id &&
                                           groupAtCourse.CourseId == req.CourseId &&
                                           !lesson.EndDate.HasValue &&
                                           userTabel.UserId == userSettings.User.Id
                                           select userTabel;

            // Получение табелей пользователя для текушего курса
            var userLessonTabel = await getUserLessonTabelsQuery.ToListAsync();

            foreach (var tabel in userLessonTabel)
            {
                tabel.HandUp = !tabel.HandUp;
                _lessonTabelRepository.Update(tabel);
            }

            await _signalRUtility.SendLessonStateSignalR(req.CourseId);
            return response;
        }
    }
}
