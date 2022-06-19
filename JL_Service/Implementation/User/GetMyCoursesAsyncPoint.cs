using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class GetMyCoursesAsyncPoint : PointBase<object?, GetMyCoursesResponse>, IGetMyCoursesAsyncPoint
    {
        private readonly IGroupAtCourseRepository _groupAtCourseRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public GetMyCoursesAsyncPoint(
            IGroupAtCourseRepository _groupAtCourseRepository,
            ICourseTeacherRepository _courseTeacherRepository,
            IUserGroupRepository _userGroupRepository,
            IUserRoleRepository _userRoleRepository,
            IRoleRepository _roleRepository,
            ICourseRepository _courseRepository,
            ApplicationContext _context) : base(_context)
        {
            this._courseRepository = _courseRepository;
            this._userRoleRepository = _userRoleRepository;
            this._roleRepository = _roleRepository;
            this._courseTeacherRepository = _courseTeacherRepository;
            this._groupAtCourseRepository = _groupAtCourseRepository;
            this._userGroupRepository = _userGroupRepository;
        }

        public override async Task<GetMyCoursesResponse> Execute(object? req, UserSettings _userSettings)
        {
            var response = new GetMyCoursesResponse();

            var userId = _userSettings.User.Id;
            var userCourses = (List<Course>?)null;

            var getUserRolesQuery = from userRole in _userRoleRepository.Get()
                                    join role in _roleRepository.Get() on userRole.RoleId equals role.Id
                                    where userRole.UserId == userId
                                    select role;
            var userRoles = await getUserRolesQuery.ToListAsync();

            // Если пользователь - преподватель
            if (userRoles.Select(x => x.SystemName).Contains(PointConsts.ROLE_TEACHER))
            {
                var getTeacherCoursesQuery = from teacherCourse in _courseTeacherRepository.Get()
                                             join course in _courseRepository.Get() on teacherCourse.CourseId equals course.Id
                                             where teacherCourse.UserId == userId
                                             select course;
                userCourses = await getTeacherCoursesQuery.ToListAsync();
            }
            else // Если пользователь - студент (или иная роль)
            {
                var getUserCoursesQuery = from userGroup in _userGroupRepository.Get()
                                          join groupAtCourse in _groupAtCourseRepository.Get() on userGroup.GroupId equals groupAtCourse.GroupId
                                          join course in _courseRepository.Get() on groupAtCourse.CourseId equals course.Id
                                          where userGroup.UserId == userId
                                          select course;
                userCourses = await getUserCoursesQuery.ToListAsync();
            }

            response.Courses = userCourses;
            response.Message = $"Доступно <{userCourses.Count}> курсов";
            response.ShowMessage = true;
            return response;
        }
    }
}
