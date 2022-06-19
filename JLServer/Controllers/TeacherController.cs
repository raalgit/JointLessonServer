using JL_ApiModels.Request.Teacher;
using JL_ApiModels.Response.Teacher;
using JL_MSSQLServer.PersistModels;
using JL_Service.Abstraction.Teacher;
using JL_Utility.Models;
using JLServer.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JLServer.Controllers
{
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TeacherController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserSettings _userSettings;

        public TeacherController(ILogger<TeacherController> logger, IServiceProvider provider)
        {
            _logger = logger;
            _serviceProvider = provider;
            _httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            _userSettings = new UserSettings((User)_httpContextAccessor.HttpContext.Items["User"]);
        }

        [HttpPost]
        [JwtAuthentication(role: "Teacher")]
        [Route("/teacher/start-sync-lesson")]
        public async Task<StartSyncLessonResponse> StartSyncLesson([FromBody] StartSyncLessonRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IStartSyncLessonPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new StartSyncLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "Teacher")]
        [Route("/teacher/close-sync-lesson")]
        public async Task<CloseLessonResponse> CloseLesson([FromBody] CloseLessonRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<ICloseLessonPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new CloseLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "Teacher")]
        [Route("/teacher/change-page")]
        public async Task<ChangeLessonManualPageResponse> ChangeActivePage([FromBody] ChangeLessonManualPageRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IChangeActivePagePoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new ChangeLessonManualPageResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }
    }
}
