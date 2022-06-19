using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Service.Exceptions;
using JL_SignalR;
using JL_Utility.Logger;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class JoinLessonAsyncPoint : PointBase<JoinLessonRequest, JoinLessonResponse>, IJoinLessonAsyncPoint
    {
        private readonly ILessonTabelRepository _lessonTabelRepository;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IJLLogger _logger;
        private readonly ISignalRUtility _signalRUtility;

        public JoinLessonAsyncPoint(
            ILessonTabelRepository _lessonTabelRepository,
            IGroupAtCourseRepository _groupAtCourseRepository,
            ILessonRepository _lessonRepository,
            ISignalRUtility _signalRUtility,
            ICourseTeacherRepository _courseTeacherRepository,
            IUserGroupRepository _userGroupRepository,
            IJLLogger _logger,
            ApplicationContext _context) : base(_context)
        {
            this._logger = _logger;
            this._lessonRepository = _lessonRepository;
            this._lessonTabelRepository = _lessonTabelRepository;
            this._groupAtCourseRepository = _groupAtCourseRepository;
            this._courseTeacherRepository = _courseTeacherRepository;
            this._signalRUtility = _signalRUtility;
            this._userGroupRepository = _userGroupRepository;
        }

        public override async Task<JoinLessonResponse> Execute(JoinLessonRequest req, UserSettings userSettings)
        {
            var response = new JoinLessonResponse();

            var courseTeachers = await _courseTeacherRepository.Get().Where(x => x.CourseId == req.CourseId).ToListAsync()
                ?? throw new PointException($"Преподаватели курса <{req.CourseId}> не найдены", _logger);

            // Если пользователь - преподаватель
            if (courseTeachers.Count > 0 && courseTeachers.Select(x => x.UserId).Contains(userSettings.User.Id))
            {
                await JoinLessonAsTeacher(courseTeachers, userSettings.User.Id, req.CourseId);
            }
            else
            {
                var userId = userSettings.User.Id;
                await CloseOldTabels(userId);
                await InsertNewTabels(userId, req.CourseId);
            }

            await _signalRUtility.SendLessonStateSignalR(req.CourseId);
            response.Message = "Вы успешно присоединились к занятию";
            return response;
        }

        /// <summary>
        /// Закрытие всех предыдущих сессий на всех занятиях
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<int> CloseOldTabels(int userId)
        {
            var oldActiveTabels = await _lessonTabelRepository.Get()
                    .Where(x => 
                    x.UserId == userId && 
                    !x.LeaveDate.HasValue)
                    .ToListAsync();

            if (oldActiveTabels != null && oldActiveTabels.Count > 0)
            {
                foreach (var tabel in oldActiveTabels)
                {
                    tabel.LeaveDate = DateTime.Now;
                    _lessonTabelRepository.Update(tabel);
                }
            }

            return 0;
        }

        /// <summary>
        /// Установка флага присутствия на занятии для преподавателя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        /// <exception cref="PointException"></exception>
        private async Task<int> JoinLessonAsTeacher(List<CourseTeacher> courseTeachers, int userId, int courseId)
        {
            var courseTeacher = courseTeachers.FirstOrDefault(x => x.UserId == userId && x.CourseId == courseId)
                    ?? throw new PointException("Запись о преподавателе курса не найдена", _logger);

            courseTeacher.OnLesson = true;
            await _courseTeacherRepository.UpdateAsync(courseTeacher);
            return 0;
        }

        /// <summary>
        /// Добавление сессий для каждой группы пользователя
        /// на занятии
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        private async Task<int> InsertNewTabels(int userId, int courseId)
        {
            var getUserLessonsQuery = from userGroup in _userGroupRepository.Get()
                                      join groupAtCourse in _groupAtCourseRepository.Get()
                                      on userGroup.GroupId equals groupAtCourse.GroupId
                                      join lesson in _lessonRepository.Get()
                                      on userGroup.Id equals lesson.GroupAtCourseId
                                      where
                                      userGroup.UserId == userId &&
                                      groupAtCourse.CourseId == courseId &&
                                      !lesson.EndDate.HasValue
                                      select lesson;

            // Получение активных занятий групп пользователя
            var userLessons = await getUserLessonsQuery.ToListAsync();
            foreach (var activeLesson in userLessons)
            {
                var newTabel = new LessonTabel()
                {
                    EnterDate = DateTime.Now,
                    LeaveDate = null,
                    LessonId = activeLesson.Id,
                    UserId = userId
                };
                _lessonTabelRepository.Insert(newTabel);
            }

            return 0;
        }
    }
}
