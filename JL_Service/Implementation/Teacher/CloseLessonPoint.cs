using JL_ApiModels.Request.Teacher;
using JL_ApiModels.Response.Teacher;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Teacher;
using JL_Service.Exceptions;
using JL_Utility.Logger;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.Teacher
{
    public class CloseLessonPoint : PointBase<CloseLessonRequest, CloseLessonResponse>, ICloseLessonPoint
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly IJLLogger _logger;

        public CloseLessonPoint(
            ILessonRepository _lessonRepository,
            IJLLogger _logger,
            IGroupAtCourseRepository _groupAtCourseRepository,
            ApplicationContext _context) : base(_context)
        {
            this._lessonRepository = _lessonRepository;
            this._groupAtCourseRepository = _groupAtCourseRepository;
            this._logger = _logger;
        }

        public override async Task<CloseLessonResponse> Execute(CloseLessonRequest req, UserSettings userSettings)
        {
            var response = new CloseLessonResponse();

            // получение незакрытых групп занятия
            var groupsAtCourse = await _groupAtCourseRepository.Get().Where(x => x.CourseId == req.CourseId && x.IsActive == true).ToListAsync()
                ?? throw new PointException($"Не найдены группы, привязанные к курсу <{req.CourseId}>", _logger);

            var groupsAtCourseIds = groupsAtCourse.Select(x => x.Id).ToList();
            var lessonsForClosing = await _lessonRepository.Get()
                .Where(x =>
                    x.GroupAtCourseId.HasValue &&
                    groupsAtCourseIds.Contains(x.GroupAtCourseId.Value) &&
                    !x.EndDate.HasValue &&
                    x.Type == PointConsts.LESSON_ONLINE_TYPE
                    )
                .ToListAsync();

            lessonsForClosing.ForEach(x => x.EndDate = DateTime.Now);
            groupsAtCourse.ForEach(x => x.IsActive = false);

            _groupAtCourseRepository.UpdateMany(groupsAtCourse);
            _lessonRepository.UpdateMany(lessonsForClosing);

            response.CanConnectToSyncLesson = false;
            response.Message = $"Занятие завершено. Кабинет закрыт";
            return response;
        }
    }
}
