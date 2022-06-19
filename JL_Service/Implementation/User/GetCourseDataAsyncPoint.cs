using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Service.Exceptions;
using JL_Utility.Logger;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class GetCourseDataAsyncPoint : PointBase<int, GetCourseDataResponse>, IGetCourseDataAsyncPoint
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJLLogger _logger;

        public GetCourseDataAsyncPoint(
            ILessonRepository _lessonRepository,
            ICourseTeacherRepository _courseTeacherRepository,
            IGroupAtCourseRepository _groupAtCourseRepository,
            IUserGroupRepository _userGroupRepository,
            IUserRepository _userRepository,
            IJLLogger _logger,
            ApplicationContext _context) : base(_context)
        {
            this._courseTeacherRepository = _courseTeacherRepository;
            this._groupAtCourseRepository = _groupAtCourseRepository;
            this._userGroupRepository = _userGroupRepository;
            this._userRepository = _userRepository;
            this._lessonRepository = _lessonRepository;
            this._logger = _logger;
        }

        public override async Task<GetCourseDataResponse> Execute(int req, UserSettings userSettings)
        {
            var response = new GetCourseDataResponse();

            // Получение преподавателей текущего курса
            var courseTeachers = await _courseTeacherRepository.Get().Where(x => x.CourseId == req).ToListAsync();
            if (courseTeachers == null || courseTeachers.Count == 0) throw new PointException("Преподаватели курса не найдены", _logger);

            response.CourseTeachers = courseTeachers;
            var isTeacher = courseTeachers.Select(x => x.UserId).Contains(userSettings.User.Id);
            
            // Получение групп, записанных на курс
            var getGroupsDataQuery = from grpAtCourse in _groupAtCourseRepository.Get()
                                     join userGroup in _userGroupRepository.Get() on grpAtCourse.GroupId equals userGroup.GroupId
                                     join user in _userRepository.Get() on userGroup.UserId equals user.Id
                                     where
                                     grpAtCourse.CourseId == req
                                     select new
                                     {
                                         grpAtCourse,
                                         user
                                     };

            var groupsData = await getGroupsDataQuery.ToListAsync();
            var groupsAtCourse = groupsData.Select(x => x.grpAtCourse).Distinct().ToList();
            if (groupsAtCourse == null || groupsAtCourse.Count == 0)
                throw new PointException($"Учебные группы курса <{req}> не найдены", _logger);

            var activeGroupsAtCourse = groupsAtCourse.Where(x => x.IsActive).ToList();
            if (isTeacher)
            {
                response.LessonIsActive = activeGroupsAtCourse.Count > 0;
            }
            else
            {
                // Номера групп, в которых учавствует студент
                var studentGroups = groupsData.Where(x => x.user.Id == userSettings.User.Id)
                    .Select(x => x.grpAtCourse.GroupId).Distinct().ToList();

                response.LessonIsActive = 
                    activeGroupsAtCourse.Count > 0 && activeGroupsAtCourse.Any(x => studentGroups.Contains(x.GroupId));
            }

            response.IsTeacher = isTeacher;
            response.ShowMessage = true;
            response.Message = $"Данные по курсу получены";
            return response;
        }
    }
}
