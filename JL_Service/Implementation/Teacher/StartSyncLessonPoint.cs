using JL_ApiModels.Request.Teacher;
using JL_ApiModels.Response.Teacher;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Teacher;
using JL_Service.Exceptions;
using JL_Utility.Logger;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.Teacher
{
    public class StartSyncLessonPoint : PointBase<StartSyncLessonRequest, StartSyncLessonResponse>, IStartSyncLessonPoint
    {
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IJLLogger _logger;

        public StartSyncLessonPoint(
            IGroupAtCourseRepository _groupAtCourseRepository,
            ILessonRepository _lessonRepository,
            IJLLogger _logger,
            ApplicationContext _context) : base(_context)
        {
            this._logger = _logger;
            this._lessonRepository = _lessonRepository;
            this._groupAtCourseRepository = _groupAtCourseRepository;
        }

        public override async Task<StartSyncLessonResponse> Execute(StartSyncLessonRequest req, UserSettings userSettings)
        {
            var response = new StartSyncLessonResponse();

            // получение группы, для которой открывается занятие
            var groupsAtCourse = await _groupAtCourseRepository.Get().FirstOrDefaultAsync(x => x.CourseId == req.CourseId && x.GroupId == req.GroupId)
                ?? throw new PointException($"Группа под номером <{req.GroupId}>, привязанная к курсу <{req.CourseId}> не найдена", _logger);

            groupsAtCourse.IsActive = true;
            _groupAtCourseRepository.Update(groupsAtCourse);

            // получение предыдущего занятия для этой группы
            var lastGroupLesson = await _lessonRepository.Get().OrderByDescending(x => x.StartDate).FirstOrDefaultAsync();
            if (lastGroupLesson != null && lastGroupLesson.EndDate == null)
            {
                lastGroupLesson.EndDate = DateTime.Now;
                _lessonRepository.Update(lastGroupLesson);
            }
            
            // создание нового занятия для группы
            var newGroupLesson = new Lesson()
            {
                StartDate = DateTime.Now,
                EndDate = (DateTime?)null,
                LastMaterialPage = string.IsNullOrEmpty(req.StartPage) ? lastGroupLesson?.LastMaterialPage : req.StartPage,
                TeacherId = userSettings.User.Id,
                Type = PointConsts.LESSON_ONLINE_TYPE,
                GroupAtCourseId = groupsAtCourse.Id,
                CourseId = req.CourseId,
            };
            await _lessonRepository.InsertAsync(newGroupLesson);

            response.CanConnectToSyncLesson = true;
            response.Message = $"Занятие успешно активировано. Кабинет открыт";
            return response;
        }
    }
}
