using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer.PersistModels;
using JL_Service.Abstraction.User;
using JL_SignalR;
using JL_Utility.Models;
using JLServer.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace JLServer.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<SignalHub> _hubContext;
        private readonly ILogger<UserController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserSettings _userSettings;

        public UserController(ILogger<UserController> logger, IServiceProvider provider, IHubContext<SignalHub> hubContext)
        {
            _logger = logger;
            _serviceProvider = provider;
            _hubContext = hubContext;
            _httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            _userSettings = new UserSettings((User)_httpContextAccessor.HttpContext.Items["User"]);
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/my-courses")]
        public async Task<GetMyCoursesResponse> GetMyCourses()
        {
            try
            {
                var point = _serviceProvider.GetService<IGetMyCoursesAsyncPoint>();
                return await point.Start(null, _userSettings);
            }
            catch (Exception er)
            {
                return new GetMyCoursesResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/file")]
        public async Task<AddNewFileResponse> AddFile(AddNewFileRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IAddNewFileAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new AddNewFileResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/get-manual-files")]
        public async Task<GetManualFilesResponse> GetManualFiles(GetManualFilesRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IGetManualFilesAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new GetManualFilesResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/file/{fileId}")]
        public async Task<GetFileResponse> GetFile([FromRoute][Required] int fileId)
        {
            try
            {
                var point = _serviceProvider.GetService<IGetFileAsyncPoint>();
                return await point.Start(fileId, _userSettings);
            }
            catch (Exception er)
            {
                return new GetFileResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/register-signal-connection/{connectionId}")]
        public async Task<RegisterSignalConnectionResponse> RegisterSignalConnection([FromRoute][Required] string connectionId)
        {
            try
            {
                var point = _serviceProvider.GetService<IRegisterSignalConnectionAsync>();
                return await point.Start(connectionId, _userSettings);
            }
            catch (Exception er)
            {
                return new RegisterSignalConnectionResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/course-data/{courseId}")]
        public async Task<GetCourseDataResponse> GetCourseData([FromRoute][Required] int courseId)
        {
            try
            {
                var point = _serviceProvider.GetService<IGetCourseDataAsyncPoint>();
                return await point.Start(courseId, _userSettings);
            }
            catch (Exception er)
            {
                return new GetCourseDataResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/start-srs-lesson")]
        public async Task<StartSRSLessonResponse> StartSRSLesson([FromBody] StartSRSLessonRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IStartSRSLessonAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new StartSRSLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/change-page-srs-lesson")]
        public async Task<ChangeSRSLessonManualPageResponse> ChangeActivePage([FromBody] ChangeSRSLessonManualPageRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IChangeActivePageAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new ChangeSRSLessonManualPageResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/close-srs-lesson")]
        public async Task<CloseSRSLessonResponse> CloseLesson(CloseSRSLessonRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<ICloseLessonAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new CloseSRSLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/get-remote-access-data")]
        public async Task<GetRemoteAccessDataResponse> GetRemoteAccessData(GetRemoteAccessDataRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IGetRemoteAccessDataAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new GetRemoteAccessDataResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/create-remote-access")]
        public async Task<CreateRemoteAccessResponse> CreateRemoteAccess(CreateRemoteAccessRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<ICreateRemoteAccessAsync>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new CreateRemoteAccessResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpGet]
        [JwtAuthentication(role: "User")]
        [Route("/user/remote-connection-list/{courseId}")]
        public async Task<GetRemoteAccessListResponse> GetRemoteAccessList([FromRoute][Required] int courseId)
        {
            try
            {
                var point = _serviceProvider.GetService<IGetRemoteAccessListAsyncPoint>();
                return await point.Start(courseId, _userSettings);
            }
            catch (Exception ex)
            {
                return new GetRemoteAccessListResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/up-hand")]
        public async Task<UpHandResponse> UpHand(UpHandRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IUpHandAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new UpHandResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/join-lesson")]
        public async Task<JoinLessonResponse> JoinLesson(JoinLessonRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<IJoinLessonAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new JoinLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/leave-lesson")]
        public async Task<LeaveLessonResponse> LeaveLesson(LeaveLessonRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<ILeaveLessonAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception er)
            {
                return new LeaveLessonResponse()
                {
                    IsSuccess = false,
                    Message = er.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/load-note")]
        public async Task<LoadNoteResponse> LoadNoteAsync(LoadNoteRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<ILoadNoteAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception ex)
            {
                return new LoadNoteResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        [HttpPost]
        [JwtAuthentication(role: "User")]
        [Route("/user/send-note")]
        public async Task<SendNoteResponse> SendNoteAsync(SendNoteRequest request)
        {
            try
            {
                var point = _serviceProvider.GetService<ISendNoteAsyncPoint>();
                return await point.Start(request, _userSettings);
            }
            catch (Exception ex)
            {
                return new SendNoteResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
