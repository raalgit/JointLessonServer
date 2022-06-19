using JL_MSSQLServer.Repository.Abstraction;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JL_SignalR
{
    public class SignalRUtility : ISignalRUtility
    {
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ILessonTabelRepository _lessonTabelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;

        public SignalRUtility(
            IHubContext<SignalHub> hubContext,
            IGroupAtCourseRepository _groupAtCourseRepository,
            ILessonRepository _lessonRepository,
            ILessonTabelRepository _lessonTabelRepository,
            IUserGroupRepository _userGroupRepository,
            ICourseTeacherRepository _courseTeacherRepository,
            ISignalUserConnectionRepository _signalUserConnectionRepository,
            IUserRepository _userRepository)
        {
            _hubContext = hubContext;
            this._groupAtCourseRepository = _groupAtCourseRepository;
            this._lessonRepository = _lessonRepository;
            this._lessonTabelRepository = _lessonTabelRepository;
            this._userRepository = _userRepository;
            this._userGroupRepository = _userGroupRepository;
            this._courseTeacherRepository = _courseTeacherRepository;
            this._signalUserConnectionRepository = _signalUserConnectionRepository;
        }

        /// <summary>
        /// Отправка SingalR нотификации
        /// </summary>
        /// <param name="type">Тип нотификации</param>
        /// <param name="message">Сообщение</param>
        /// <param name="connectionId">Номер соединения пользователя</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task SendMessage(SignalType type, string message, string connectionId)
        {
            string content = null;
            switch (type)
            {
                case SignalType.POSTED:
                    content = "Posted";
                    break;
                default:
                    throw new ArgumentException(nameof(type));
            }

            try
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(content, message);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }

        /// <summary>
        /// Отправка SignalR нотификации о состоянии занятия
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<bool> SendLessonStateSignalR(int courseId)
        {
            var usersOnLessonSignalRModel = new List<UserAtLesson>();

            var getGroupDataQuery = from grpAtCourse in _groupAtCourseRepository.Get()
                                    join userGroup in _userGroupRepository.Get() on grpAtCourse.GroupId equals userGroup.GroupId
                                    join user in _userRepository.Get() on userGroup.UserId equals user.Id
                                    join les in _lessonRepository.Get() on grpAtCourse.Id equals les.GroupAtCourseId
                                    join lesTbl in _lessonTabelRepository.Get() on les.Id equals lesTbl.LessonId
                                    where
                                    grpAtCourse.CourseId == courseId &&
                                    grpAtCourse.IsActive &&
                                    !les.EndDate.HasValue &&
                                    !lesTbl.LeaveDate.HasValue &&
                                    lesTbl.UserId == user.Id
                                    select new
                                    {
                                        grpAtCourse,
                                        lesTbl,
                                        user
                                    };

            var getTeachersQuery = from courseTeacher in _courseTeacherRepository.Get()
                                   join user in _userRepository.Get() on courseTeacher.UserId equals user.Id
                                   where
                                   courseTeacher.CourseId == courseId &&
                                   courseTeacher.OnLesson
                                   select user;

            // получение списка учащихся на занятии
            var groupsData = await getGroupDataQuery.ToListAsync();

            // добавление списка преподавателей на занятии
            var teachersData = await getTeachersQuery.ToListAsync();

            usersOnLessonSignalRModel.AddRange(groupsData.Select(x => new UserAtLesson()
            {
                UserId = x.user.Id,
                IsTeacher = false,
                UpHand = x.lesTbl.HandUp,
                UserFio = x.user.FirstName + " " + x.user.ThirdName
            }).ToList());
            usersOnLessonSignalRModel.AddRange(teachersData.Select(x => new UserAtLesson()
            {
                UserId = x.Id,
                IsTeacher = true,
                UpHand = false,
                UserFio = x.FirstName + " " + x.ThirdName
            }).ToList());

            // Получение номеров студентов и преподавателей
            var userIds = groupsData.Select(x => x.user).Select(x => x.Id).Distinct().ToList();
            userIds.AddRange(teachersData.Select(x => x.Id));

            // отправка нотификации
            var connectionInfo = _signalUserConnectionRepository.Get().Where(x => userIds.Contains(x.UserId)).ToList();
            var connectionIds = connectionInfo.Select(x => x.ConnectionId).ToArray();

            // отправка signalR нотификации об изменении страницы всем участникам групп
            string lessonUsersJson = JsonSerializer.Serialize(usersOnLessonSignalRModel);
            await _hubContext.Clients.Clients(connectionIds).SendAsync("LessonUsersUpdate", lessonUsersJson);

            return true;
        }


        [Serializable]
        class UserAtLesson
        {
            public int UserId { get; set; }
            public string UserFio { get; set; }
            public bool UpHand { get; set; }
            public bool IsTeacher { get; set; }
        }
    }
}
