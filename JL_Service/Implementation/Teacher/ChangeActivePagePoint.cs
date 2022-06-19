using JL_ApiModels.Request.Teacher;
using JL_ApiModels.Response.Teacher;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Teacher;
using JL_SignalR;
using JL_Utility.Models;
using Microsoft.AspNetCore.SignalR;

namespace JL_Service.Implementation.Teacher
{
    public class ChangeActivePagePoint : PointBase<ChangeLessonManualPageRequest, ChangeLessonManualPageResponse>, IChangeActivePagePoint
    {
        private readonly IUserRepository _userRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;
        private readonly IHubContext<SignalHub> _hubContext;

        public ChangeActivePagePoint(
            IUserRepository _userRepository,
            ILessonRepository _lessonRepository,
            IUserGroupRepository _userGroupRepository,
            IGroupAtCourseRepository _groupAtCourseRepository,
            ISignalUserConnectionRepository _signalUserConnectionRepository,
            IGroupRepository _groupRepository,
            IHubContext<SignalHub> hubContext,
            ApplicationContext _context
            ) : base(_context)
        {
            this._userRepository = _userRepository;
            this._lessonRepository = _lessonRepository;
            this._groupAtCourseRepository = _groupAtCourseRepository;
            this._hubContext = hubContext;
            this._groupRepository = _groupRepository;
            this._userGroupRepository = _userGroupRepository;
            this._signalUserConnectionRepository = _signalUserConnectionRepository;
        }

        public override async Task<ChangeLessonManualPageResponse> Execute(ChangeLessonManualPageRequest req, UserSettings userSettings)
        {
            var response = new ChangeLessonManualPageResponse();

            var getGroupsDataQuery = from grpAtCourse in _groupAtCourseRepository.Get()
                                     join userGroup in _userGroupRepository.Get() on grpAtCourse.GroupId equals userGroup.GroupId
                                     join user in _userRepository.Get() on userGroup.UserId equals user.Id
                                     where 
                                     grpAtCourse.CourseId == req.CourseId &&
                                     grpAtCourse.IsActive
                                     select new
                                     {
                                         grpAtCourse,
                                         user
                                     };

            // получение списка групп и участников групп, которые учавствуют в занятии
            var groupsData = getGroupsDataQuery.ToList();

            // получение групп текущего курса
            var groupsOnLesson = groupsData.Select(x => x.grpAtCourse).Distinct().ToList();

            // Получение участников группы текущего курса
            var usersOnLesson = groupsData.Select(x => x.user).Distinct().ToList();

            // Если синхронизация главного окна
            if (req.ForMainWindows)
            {
                var groupsAtCourseIds = groupsOnLesson.Select(x => x.Id);

                // получение списка активных занятий текущего курса
                var activeLessons = _lessonRepository.Get()
                    .Where(x =>
                        x.GroupAtCourseId.HasValue &&
                        groupsAtCourseIds.Contains(x.GroupAtCourseId.Value) &&
                        !x.EndDate.HasValue &&
                        x.Type == PointConsts.LESSON_ONLINE_TYPE
                        )
                    .ToList();

                // установка текущей страницы для всех занятий
                activeLessons.ForEach(x => x.LastMaterialPage = req.NextPage);
                _lessonRepository.UpdateMany(activeLessons);
            }

            var userIds = usersOnLesson.Select(x => x.Id);

            // получение списка signalR соединений для участников групп
            var connectionInfo = _signalUserConnectionRepository.Get().Where(x => userIds.Contains(x.UserId)).ToList();
            var connectionIds = connectionInfo.Select(x => x.ConnectionId).ToArray();

            // отправка signalR нотификации об изменении страницы всем участникам групп
            await _hubContext.Clients.Clients(connectionIds).SendAsync("PageSync", req.NextPage + "/" + req.ForMainWindows.ToString());

            response.Message = $"Текущая страница изменена на {req.NextPage} с синхронизацией";
            response.ShowMessage = false;
            return response;
        }
    }
}
